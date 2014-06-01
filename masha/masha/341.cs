using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace masha2
{
    //прототип
    class Program
    {
        static void Main(string[] args)
        {
            Folder g = new Folder(new List<File> { new File(), new File(), new File(), new File() });
            Console.WriteLine("\\-------deep copy-------//");
            Console.WriteLine("File #2 marker is " + g[1].Marker);
            Folder cg = g.Clone() as Folder;
            g[1].Marker = 27;
            Console.WriteLine("It is cloned\nits marker is chanched to 27");
            Console.WriteLine("its clone's marker is " + cg[1].Marker);

            Console.WriteLine("\\-------shalow copy-------//");
            Console.WriteLine("File #2 marker is " + g[1].Marker);
             cg = g.ShallowCopy() as Folder;
            g[1].Marker = 17;
            Console.WriteLine("It is cloned\nits marker is chanched to 17");
            Console.WriteLine("its clone's marker is " + cg[1].Marker);
            Console.Read();

        }

        public class File : ICloneable
        {
            private static Random rnd = new Random();
            public File()
            {
                this.age = rnd.Next(25);
            }

            string content;
            int age;
            public int Marker;

            public object Clone()
            {
                return this.MemberwiseClone();
            }
        }


        public class Folder : ICloneable
        {
            public Folder(List<File> lst)
            {
                this.lst = lst;
            }
            List<File> lst = new List<File>();

            public File this[int i]
            {
                get { return i >= this.lst.Count ? null : this.lst[i]; }
            }


            //deep copy
            public object Clone()
            {
                List<File> clone_lst = new List<File>();

                foreach (var s in this.lst)
                    clone_lst.Add(s.Clone() as File);

                return new Folder(clone_lst);
            }

            public object ShallowCopy()
            {
                return this.MemberwiseClone();
            }
        }
    }
}
