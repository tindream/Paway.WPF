using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paway.WPF
{
    internal class ElementData<T, I> where T : class where I : struct
    {
        public T Old { get; set; }
        public I? Normal { get; set; }
        public I? Mouse { get; set; }
        public I? Pressed { get; set; }
        public int? Alpha { get; set; }

        public ElementData() { }
        public ElementData(T old, I? normal, I? mouse, I? pressed, int? alpha)
        {
            this.Old = old;
            this.Normal = normal;
            this.Mouse = mouse;
            this.Pressed = pressed;
            this.Alpha = alpha;
        }
    }
}
