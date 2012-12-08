namespace ExportTest

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open ExportF

type FakePdfExport() =
    inherit PdfExport("Test Export", "Title", "Header", DateTime.Now.ToString(), "My Org", ".\\test.pdf", Seq.empty)
    override x.BuildDocument members =
        new Text.StringBuilder()


[<TestClass>]
type TestPdfExport() =
    [<TestMethod>]
    member x.TestMethod1 () =
        let export = FakePdfExport()
        Assert.AreEqual(1, export)
