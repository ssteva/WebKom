import {inject} from 'aurelia-framework'
import {Endpoint} from 'aurelia-api';
//import {EntityManager} from 'aurelia-orm';
@inject(Endpoint.of())
export class Common {

  constructor(repo){
    this.repo = repo;
  }

  roles = ["Komercijalista", "Supervizor", "Administrator"];
  filterMenu(e){
    //e.sender.dataSource.options.schema.model.fields[e.field].type == "date"
    if (e.field.includes("datum")) {
        let beginOperator = e.container.find("[data-role=dropdownlist]:eq(0)").data("kendoDropDownList");
        beginOperator.value("gte");
        beginOperator.trigger("change");

        let endOperator = e.container.find("[data-role=dropdownlist]:eq(2)").data("kendoDropDownList");
        endOperator.value("lte");
        endOperator.trigger("change");
        
        e.container.find(".k-dropdown").hide();
    }
  };
  datumDokumentaFilter =
  {
      extra : true,
      messages: {
          info: "Period od - do" // sets the text on top of the Filter menu
      },
      ui: (element) => {
          element.kendoDatePicker({
              format: "dd.MM.yyyy"
          });
      }
  };
  pageable = {
    messages: {
      display: "{0} - {1} of {2} redova", //{0} is the index of the first record on the page, {1} - index of the last record on the page, {2} is the total amount of records
      empty: "Broj redova za prikaz",
      page: "Strana",
      of: "od {0}", //{0} is total amount of pages
      itemsPerPage: "redova po strani",
      first: "Idi na prvu stranu",
      previous: "Idi na poslednju stranu",
      next: "Idi na sledeću stranu",
      last: "Idi na poslednju stranu",
      refresh: "Osveži"
    },
    refresh: true,
    pageSizes: true,
    buttonCount: 5
  };
  filterobj = {
    messages: {
      info: "Prikaži podatke koji", // sets the text on top of the Filter menu
      filter: "Filter", // sets the text for the "Filter" button
      clear: "Poništi", // sets the text for the "Clear" button

      // when filtering boolean numbers
      isTrue: "custom is true", // sets the text for "isTrue" radio button
      isFalse: "custom is false", // sets the text for "isFalse" radio button

      //changes the text of the "And" and "Or" of the Filter menu
      and: "CustomAnd",
      or: "CustomOr"
    },
    extra: false,
    operators: {
      string: {
        contains: "Sadrže"
      }
    }
  };
  dsMestoIsporuke = new kendo.data.DataSource({
    pageSize: 10,
    batch: false,
    transport: {
      read: (o) => {
        let filter = "";
        if (o.data.filter && o.data.filter.filters && o.data.filter.filters.length > 0) {
          filter = o.data.filter.filters[0].value;
        }
        this.repo.find('Subjekat/ListaKupacaComboSp?filter=' + filter + "&id=")
          .then(result => {
            o.success(result);
          })
          .catch(err => {
            console.log(err.statusText);
          });

      }
    },
    serverPaging: true,
    serverSorting: true,
    serverFiltering: true,
  });
  dsKupac = new kendo.data.DataSource({
    pageSize: 10,
    batch: false,
    transport: {
      read: (o) => {
        let filter = "";
        if (o.data.filter && o.data.filter.filters && o.data.filter.filters.length > 0) {
          filter = o.data.filter.filters[0].value;
        }
        this.repo.find('Subjekat/ListaKupacaComboSp?filter=' + filter)
          .then(result => {
            o.success(result);
          })
          .catch(err => {
            console.log(err.statusText);
          });
      }
    },
    serverPaging: true,
    serverSorting: true,
    serverFiltering: true,
  });
  mestoIsporukeFilter =
  {
      extra: false,
      ui: (element) => {
          element.kendoComboBox({
              dataTextField: "sifra",
              dataValueField: "id",
              filter: "contains",
              dataSource: this.dsKupac
          });
      },
      operators: {
          string: {
              contains: "Sadrže"
      },
          number: {
              eq: "je jednak"
          }
      }
  };
  kupacFilter =
  {
      extra: false,
      ui: (element) => {
          element.kendoComboBox({
              dataTextField: "sifra",
              dataValueField: "id",
              filter: "contains",
              dataSource: this.dsKupac
          });
      },
      operators: {
          string: {
              contains: "Sadrže"
      },
          number: {
              eq: "je jednak"
          }
      }
  };
}
