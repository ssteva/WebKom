using System;
using Newtonsoft.Json;

namespace webkom.Models
{
  public class MeniSettings 
  {
    public MeniSettings()
    {

    }
    public MeniSettings(string Ikona, string RolesString)
    {
      this.Ikona = Ikona;
      this.RolesString = RolesString;
    }
    public virtual int Id { get; set; }
    //public virtual Meni Meni { get; set; }
    public virtual string Ikona { get; set; }
    //public virtual Meni Meni { get; set; }
    [JsonIgnore]
    public virtual string RolesString { get; set; }

    public virtual string[] Roles
    {
      get
      {
        return RolesString.Split(',');
      }
    }
  }
}
