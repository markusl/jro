namespace ExportF

open System
open System.Text
open System.ComponentModel

open Fonet

open Database

[<AbstractClass>]
type public PdfExport(name, title, header, date, organization, path:string, members) =
    abstract member BuildDocument : seq<Member> -> StringBuilder
    static member CreateTempFile (contents:string) =
        let tempFile = IO.Path.GetTempFileName()
        IO.File.WriteAllText(tempFile, contents)
        tempFile

    interface IPdfExport with
        /// Name (description) of this exporter
        member x.Name with get() : string = name
        member x.DocumentTitle with get() : string = title
        member x.DocumentHeader with get() : string = header
        member x.DocumentDate with get() : string = date
        member x.OrgName with get() = ""
        /// The list of members to include in this export.
        member x.Members list = Seq.empty
        member x.Export processChanged = x.BuildDocument members |> x.renderDocument processChanged
        member x.ToString = failwith "Not implemented"
        /// Path to the output file.
        member x.OutputPath with get() = path
        /// Fired when progress is made in rendering the output.
        member x.ProgressChanged : EventHandler<ProgressChangedEventArgs> = null
        /// Happens when the export is completed.
        member x.ExportCompleted : EventHandler = null

    /// Report progress based on Fonet driver's output
    member private x.driverOnInfo members pagesPer100Members processChanged (ev:FonetEventArgs) =
        let getProgressFromPage(message:string) =
            let currentPage = Int32.Parse(message.Substring(1, message.Length - 2))
            let expectedPages = Math.Max((members / 100), 1) * pagesPer100Members
            let pageProgress = float (currentPage / expectedPages)
            Math.Min(Math.Max(7., pageProgress), 1.);

        let message = ev.GetMessage()
        let progress =
            if message.Equals("Building formatting object tree") then 0.05
            else if(message.StartsWith("[") && message.EndsWith("]")) then
                getProgressFromPage(message)
            else -1.
        match progress with
            | -1. -> ()
            | percentage -> processChanged percentage

    member x.renderDocument processChanged document =
        try
            let options = new Fonet.Render.Pdf.PdfRendererOptions(Author = organization,
                                                                  Title = title)
            let driver = new FonetDriver(Options = options)
            let tempFile = PdfExport.CreateTempFile(document.ToString())
            driver.Render(tempFile, path)
            driver.OnInfo.Add(x.driverOnInfo (members|>Seq.length) 1 processChanged)
            IO.File.Delete tempFile
            processChanged 1.0
        with | e -> Diagnostics.Debug.Print e.Message
