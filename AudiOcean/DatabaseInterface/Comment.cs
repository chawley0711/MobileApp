//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DatabaseInterface
{
    using System;
    using System.Collections.Generic;
    
    public partial class Comment
    {
        public int CommentID { get; set; }
        public Nullable<int> SongID { get; set; }
        public string Text { get; set; }
        public System.DateTime DatePosted { get; set; }
        public Nullable<int> UserID { get; set; }
    
        public virtual Song Song { get; set; }
        public virtual User User { get; set; }
    }
}