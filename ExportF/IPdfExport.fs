namespace ExportF

open System
open System.Collections.Generic
open System.Linq
open System.Text
open System.ComponentModel

open Database

/// Interface for implementing PDF exporters.
type public IPdfExport = 
    /// Name (description) of this exporter
    abstract member Name : string with get
    abstract member DocumentTitle : string with get
    abstract member DocumentHeader : string with get
    abstract member DocumentDate : string with get
    abstract member OrgName : string with get
    /// The list of members to include in this export.
    abstract Members : IPdfExport -> seq<Member>
    abstract Export : (float -> unit) -> unit
    abstract ToString : string
    /// Path to the output file.
    abstract member OutputPath : string with get
    /// Fired when progress is made in rendering the output.
    abstract ProgressChanged : EventHandler<ProgressChangedEventArgs>
    /// Happens when the export is completed.
    abstract ExportCompleted : EventHandler
