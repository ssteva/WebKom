using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace webkom.Models
{
  public class Meni 
  {
    public Meni()
    {
      Podmeni = new HashSet<Meni>();
    }
    public Meni(int Rbr, string RouteString, string Name, string Href, string ModuleId, string Title, bool Nav, bool Auth, MeniSettings meniSettings, Meni master)
    {
      Podmeni = new HashSet<Meni>();
      this.Rbr = Rbr;
      this.RouteString = RouteString;
      this.Name = Name;
      this.Href = Href;
      this.ModuleId = ModuleId;
      this.Title = Title;
      this.Nav = Nav;
      this.Auth = Auth;
      this.Master = master;
      Settings = meniSettings;
    }
    public virtual int Id { get; set; }
    public virtual int Rbr { get; set; }
    [JsonIgnore]
    public virtual string RouteString { get; set; }
    public virtual string Name { get; set; }
    public virtual string Href { get; set; }
    public virtual string ModuleId { get; set; }
    public virtual string Title { get; set; }
    public virtual bool Nav { get; set; }
    public virtual bool Auth { get; set; }
    public virtual MeniSettings Settings { get; set; }
    public virtual Meni Master {get;set;}
    public virtual ICollection<Meni> Podmeni {get;set;}

    public virtual string[] Route
    {
      get
      {
        return RouteString.Split(',');
      }
    }
    
  }
}
