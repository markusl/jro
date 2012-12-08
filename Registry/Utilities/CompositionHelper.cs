using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;

namespace Registry.Utilities
{
    /// <summary>
    /// Helper utility for composing objects.
    /// </summary>
    public static class CompositionHelper
    {
        private static AggregateCatalog catalog = new AggregateCatalog();

        static CompositionHelper()
        {
            catalog.Catalogs.Add(new AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly()));
        }

        public static void ComposeInExecutingAssembly(object objectToCompose)
        {
            using(var container = new CompositionContainer(catalog))
                DoCompose(container, BatchFrom(objectToCompose));
        }

        public static void ComposeInExecutingAssemblyWithExported<T>(object objectToCompose)
        {
            using(CompositionContainer cc = new CompositionContainer(catalog))
            {
                cc.ComposeExportedValue<T>((T)objectToCompose);
                DoCompose(cc, BatchFrom(objectToCompose));
            }
        }
        private static CompositionBatch BatchFrom(object objectToCompose)
        {
            CompositionBatch batch = new CompositionBatch();
            batch.AddPart(objectToCompose);
            return batch;
        }

        private static CompositionBatch BatchFrom(object o1, object o2)
        {
            CompositionBatch batch = new CompositionBatch();
            batch.AddPart(o1);
            batch.AddPart(o2);
            return batch;
        }

        /// <summary>
        /// Do the compose for the bacthObject with two parameters.
        /// </summary>
        public static void ComposeInAssemblyWithExported<T1, T2>(object batchObject, T1 param1, T2 param2, System.Reflection.Assembly assembly)
        {
            var cat = new AssemblyCatalog(assembly);
            try
            {
                catalog.Catalogs.Add(cat);
                using(CompositionContainer cc = new CompositionContainer(catalog))
                {
                    cc.ComposeExportedValue(param1);
                    cc.ComposeExportedValue(param2);
                    DoCompose(cc, BatchFrom(batchObject));
                }
            }
            finally
            {
                catalog.Catalogs.Remove(cat);
            }
        }

        private static void DoCompose(CompositionContainer cc, CompositionBatch batch)
        {
            cc.Compose(batch);
        }
    }
}
