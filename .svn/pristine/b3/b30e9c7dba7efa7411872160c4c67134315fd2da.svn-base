using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SchoolWebApi.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class DynamicXml : DynamicObject, IEnumerable
    {
        private readonly List<XElement> _elements;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public DynamicXml(string text)
        {
            var doc = XDocument.Parse(text);
            _elements = new List<XElement> { doc.Root };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        protected DynamicXml(XElement element)
        {
            _elements = new List<XElement> { element };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elements"></param>
        protected DynamicXml(IEnumerable<XElement> elements)
        {
            _elements = new List<XElement>(elements);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;
            if (binder.Name == "Value")
                result = _elements[0].Value;
            else if (binder.Name == "Count")
                result = _elements.Count;
            else
            {
                var attr = _elements[0].Attribute(XName.Get(binder.Name));
                if (attr != null)
                    result = attr;
                else
                {
                    var items = _elements.Descendants(XName.Get(binder.Name));
                    if (items == null || !items.Any())
                        return false;
                    result = new DynamicXml(items);
                }
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="indexes"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            int ndx = (int)indexes[0];
            result = new DynamicXml(_elements[ndx]);
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            foreach (var element in _elements)
                yield return new DynamicXml(element);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (_elements.Count == 1 && !_elements[0].HasElements)
            {
                return _elements[0].Value;
            }

            return string.Join("\n", _elements);
        }
    }
}
