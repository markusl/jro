namespace ExportF

open System
open System.Collections.Generic
open System.Linq
open System.Text
open System.ComponentModel

/// Interface for module that handles queuing and progress tracking of export jobs.
type IExportService = 
    /// Queue a job to be exported.
    abstract QueueExportJob : IPdfExport
    /// Check whether the application is currently exporting something.
    abstract member ExportInProgress : bool with get, set
    /// Get the percentage of export progress completed.
    abstract member ExportProgress : bool with get, set
    /// Get the name of document currently being exported, or null if none.
    abstract member CurrentDocument : string with get, set
    /// Event that will be raised when all exports are completed.
    abstract AllExportsCompleted : EventHandler
