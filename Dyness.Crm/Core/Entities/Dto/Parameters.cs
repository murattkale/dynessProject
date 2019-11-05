using System.ComponentModel;
using System.Data;

namespace Core.Entities.Dto
{
    public class Parameter
    {
        public Parameter()
        {

        }

        public Parameter(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }

        public object Value { get; set; }

        [DefaultValue(ParameterDirection.Input)]
        public ParameterDirection Direction { get; set; }
    }
}
