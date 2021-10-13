using ltrModulesNet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCore.LTRManager;

namespace LTRManager.Model
{
    public enum TypeNode
    {
        /// <summary>
        /// Сам ltrd сервис
        /// </summary>
        Ltrd,
        /// <summary>
        /// Крейт
        /// </summary>
        Crate,
        /// <summary>
        /// Лкарта
        /// </summary>
        LCard
    }
    public class NodeObject : BindableBase
    {
        /// <summary>
        /// Название узла
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Тип узла
        /// </summary>
        public TypeNode TypeNode { get; }
        /// <summary>
        /// Дочерние узлы
        /// </summary>
        public ObservableCollection<NodeObject> Nodes { get; set; }
        /// <summary>
        /// Объект для крейтов, карт и сервиса
        /// </summary>
        public object Object { get; set; }
        /// <summary>
        /// Родительский узел в дереве
        /// </summary>
        public NodeObject ParantNode { get; set; }
        public NodeObject(string name, TypeNode type)
        {
            Name = name;
            TypeNode = type;
            Nodes = new ObservableCollection<NodeObject>();
            Object = null;
        }

        /// <summary>
        /// Создание для лтрд сервиса
        /// </summary>
        /// <param name="ltrsrvcon"></param>
        public NodeObject(ltrsrvcon ltrsrvcon, string ver, NodeObject parent)
        {
            ParantNode = parent;
            LtrdServerObject ltrdServerObject = new LtrdServerObject(ltrsrvcon, ver);
            Object = ltrdServerObject;
            Name = ltrdServerObject.Name;
            TypeNode = TypeNode.Ltrd;
            Nodes = new ObservableCollection<NodeObject>();
        }

        /// <summary>
        /// Создание для крейта
        /// </summary>
        /// <param name="type"></param>
        /// <param name="serial"></param>
        /// <param name="typeNode"></param>
        public NodeObject(ltrcrate crate, NodeObject parent, string csn, IPAddressCrateObject ipObj, LTRManagerSyncCrateSettings setting)
        {
            ParantNode = parent;
            CrateObject crateObject = new CrateObject(crate, csn, ipObj, setting);
            Object = crateObject;
            Name = crateObject.Name + " (" + crateObject.Serial + ")";
            TypeNode = TypeNode.Crate;
            Nodes = new ObservableCollection<NodeObject>();
        }

        public NodeObject(_LTRNative.MODULETYPE module, int slot, NodeObject parent)
        {
            ParantNode = parent;
            LCardObject lCardObject = new LCardObject(module, slot, parent.Object as CrateObject);
            Object = lCardObject;
            Name = lCardObject.Slot + " : " + lCardObject.Name;
            TypeNode = TypeNode.LCard;
            Nodes = null;
        }

        public List<CrateObject> GetCratesObjects(NodeObject node)
        {
            List<CrateObject> crates = new List<CrateObject>();
            CrateObject crate = node.Object as CrateObject;
            if (crate != null)
                crates.Add(crate);
            foreach (var _node in node.Nodes ?? new ObservableCollection<NodeObject>()) 
            {
                crates.AddRange(GetCratesObjects(_node));
            }
            return crates;
        }
    }
}
