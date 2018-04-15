//import {inject} from 'aurelia-framework'
//import {Endpoint} from 'aurelia-api';
//import {EntityManager} from 'aurelia-orm';
//@inject(Endpoint.of())
export class Common {
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
}
