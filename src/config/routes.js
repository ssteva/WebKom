export default [
    { rbr: 2, route: 'test', name: 'test', moduleId: 'test', nav: false, title: 'Test', auth:true,settings: { roles: ['Administrator'] }},
    { route: 'login', name: 'login', moduleId: 'login', nav: false, title:'Login'},
    {rbr: 1, route: ['', 'home'], name: 'home', moduleId: 'pages/home/home', nav: false, title: 'Home', settings: {ikona:'home',roles: ['Administrator','Supervizor','Komercijalista']}, auth:true},
    { route: 'autocomplete',  name: 'autocomplete', moduleId: 'pages/autocomplete/autocomplete', nav: true, title: 'Autocomplete' }
    
    //{ route: ['nalog/:id'], name: 'nalog', moduleId: 'nalog', nav: false, title: 'Radni nalog',href:'#nalog', settings: {ikona:'build',roles: ['Administrator', 'Interni serviser', 'Eksterni serviser', 'Koordinator', 'Magacioner', 'Call centar', 'Šef servisa']}, auth:true},
    //{ route: ['nalozi'], name: 'nalozi', moduleId: 'nalozi', nav: true, title: 'Pregled naloga',href:'#nalozi', settings: {ikona:'view_list',roles: ['Administrator', 'Interni serviser', 'Eksterni serviser', 'Koordinator', 'Magacioner', 'Call centar', 'Šef servisa']}, auth:true},
    //{ route: ['mis'], name: 'mis', moduleId: 'mis', nav: true, title: 'Mes. izveštaj servisa',href:'#mis', settings: {ikona:'view_list',roles: ['Administrator', 'Koordinator', 'Šef servisa']}, auth:true},
    //{ route: ['prijem/:id'], name: 'prijem', moduleId: 'prijem', nav: false, title: 'Novi prijemnica',href:'#prijem', settings: {ikona:'build',roles: ['Administrator','Koordinator']}, auth:true},
    //{ route: ['prijemi'], name: 'prijemi', moduleId: 'prijemi', nav: true, title: 'Pregled prijemnica',href:'#prijemi', settings: {ikona:'view_list',roles: ['Administrator', 'Koordinator']}, auth:true},
    //{ route: ['otpremnice'], name: 'otpremnice', moduleId: 'otpremnice', nav: true, title: 'Pregled otpremnica',href:'#otpremnice', settings: {ikona:'view_list',roles: ['Administrator', 'Magacioner']}, auth:true},
    //{ route: ['porudzbenicevp'], name: 'porudzbenicevp', moduleId: 'porudzbenice', nav: true, title: 'Pregled porudžbenica',href:'#porudzbenicevp', settings: {ikona:'view_list',roles: ['Administrator','Eksterni serviser', 'Koordinator'], vrsta:'VP'}, auth:true},
    //{ route: ['porudzbenicare'], name: 'porudzbenicare', moduleId: 'porudzbenice', nav: true, title: 'Pregled reversa',href:'#porudzbenicare', settings: {ikona:'view_list',roles: ['Administrator','Eksterni serviser', 'Koordinator'], vrsta:'RE'}, auth:true},
    //{ route: ['porudzbenicemp'], name: 'porudzbenicemp', moduleId: 'porudzbenice', nav: true, title: 'Maloprodaja',href:'#porudzbenicemp', settings: {ikona:'view_list',roles: ['Administrator','Eksterni serviser', 'Koordinator'], vrsta:'MP'}, auth:true},
    //{ route: ['porudzbenica/:id/:vrsta?'], name: 'porudzbenica', moduleId: 'porudzbenica', nav: false, title: 'Porudžbenica',href:'#porudzbenica', settings: {ikona:'build',roles: ['Administrator','Eksterni serviser', 'Koordinator']}, auth:true},
    //{ route: ['trebovanja'], name: 'trebovanja', moduleId: 'trebovanja', nav: true, title: 'Aktivna trebovanja',href:'#trebovanja', settings: {ikona:'view_list',roles: ['Administrator', 'Magacioner']}, auth:true},
    //{ route: ['otpremnica/:id'], name: 'otpremnica', moduleId: 'otpremnica', nav: false, title: 'Otpremnica',href:'#otpremnica', settings: {ikona:'build',roles: ['Administrator','Magacioner']}, auth:true},
    //{ route: ['karticadelova'], name: 'karticadelova', moduleId: 'karticadelova', nav: true, title: 'Magacinska kartica',href:'#karticadelova', settings: {ikona:'view_list',roles: ['Administrator', 'Magacioner']}, auth:true},
    //{ route: ['spisaktrebovanja'], name: 'spisaktrebovanja', moduleId: 'spisaktrebovanja', nav: true, title: 'Spisak trebovanja',href:'#spisaktrebovanja', settings: {ikona:'view_list',roles: ['Administrator', 'Koordinator',]}, auth:true},
    //{ route: 'login', name: 'login', moduleId: 'login', nav: false, title:'Login', authRoute: true },
    //{ route: 'sifarnici', name: 'sifarnici', moduleId: 'sifarnici', nav: true, title: 'Šifarnici',settings:{'ikona':'list',
    //    subMenu:[
    //      {href:'#/sub1', title:'Submenu 1'},
    //      {href:'#/sub2', title:'Submenu 2'},
    //      {href:'#/sub3', title:'Submenu 3'}
    //    ]
    //} },
    //{ route: 'login',      name: 'Login',   moduleId: 'login',          nav: false, title: 'login',settings: { roles: [] } },
    //{ route: 'grad', name: 'grad', moduleId: 'administracija/grad', nav: false, title: 'Gradovi',settings: { roles: ['Administrator'] } , auth:true},
    //{ route: 'korisnik', name: 'korisnik', moduleId: 'administracija/korisnik', nav: false, title: 'Korisnici', auth:true,settings: { roles: ['Administrator'] }},
    //{ route: 'servis', name: 'servis', moduleId: 'administracija/servis', nav: false, title: 'Servisi', auth:true,settings: { roles: ['Administrator'] }},
    //{ route: 'rezervnideo', name: 'rezervnideo', moduleId: 'administracija/rezervnideo', nav: false, title: 'Rezervni delovi', auth:true,settings: { roles: ['Administrator'] }},
    //{ route: 'administracija', name: 'administracija', moduleId: 'administracija', nav: true, title: 'Administracija',settings:{ikona: 'list', roles: ['Administrator'] ,
    //    subMenu:[
    //    {href:'#/grad', title:'Gradovi'},
    //    {href:'#/korisnik', title:'Korisnici'},
    //    {href:'#/servis', title:'Servisi'},
    //    {href:'#/rezervnideo', title:'Rezervni delovi'}
    //    ]
    //} }
    //{ route: 'users', name: 'users', moduleId: 'users', nav: true, title: 'Github Users' },
    //{ route: 'child-router', name: 'child-router', moduleId: 'child-router', nav: true, title: 'Child Router' },
    //{ route: 'k-autocomplete', name: 'k-button', moduleId: 'kendoui/autocomplete/k-autocomplete', nav: true, title: 'KendoUI autocomplete' },
    //{ route: 'k-button', name: 'k-button', moduleId: 'kendoui/button/k-button', nav: true, title: 'KendoUI button' },
    //{ route: 'k-toolbar', name: 'k-toolbar', moduleId: 'kendoui/toolbar/k-toolbar', nav: true, title: 'KendoUI toolbar' },
    //{ route: 'k-chart',         name: 'k-chart',      moduleId: 'kendoui/chart/k-chart',                nav: true, title: 'KendoUI chart' },
    //{ route: 'k-grid',          name: 'k-grid',       moduleId: 'kendoui/grid/k-grid',                  nav: true, title: 'KendoUI grid' },
    //{ route: 'k-scheduler',     name: 'k-scheduler',  moduleId: 'kendoui/scheduler/k-scheduler',        nav: true, title: 'KendoUI scheduler' },
    //{ route: 'k-treeview',      name: 'k-treeview',   moduleId: 'kendoui/treeview/k-treeview',          nav: true, title: 'KendoUI treeview' },
    //{ route: 'nalog/:id',      name: 'Nalog',   moduleId: 'nalog', href:'#nalog',          nav: true, title: 'Nalog  proba' },
    //{ route: 'kupci',      name: 'Kupci',   moduleId: 'kupci',          nav: true, title: 'Kupci proba' }
    //{ route: 'proba',      name: 'Proba',   moduleId: 'proba',          nav: true, title: 'proba' }
  //{
  //  route   : '',
  //  name    : 'index',
  //  moduleId: 'page/index',
  //  nav     : true,
  //  auth    : true,
  //  title   : 'Home'
  //},
  //{
  //  route   : '/login',
  //  name    : 'login',
  //  moduleId: 'page/auth/login',
  //  nav     : true,
  //  auth    : false,
  //  title   : 'Login'
  //},
  //{
  //  route: 'logout',
  //  name: 'logout',
  //  moduleId: 'page/auth/logout',
  //  title: 'Logout'
  //},
  //{
  //  route   : '/lists',
  //  name    : 'lists',
  //  moduleId: 'page/todo/list',
  //  nav     : true,
  //  auth    : true,
  //  title   : 'Todo lists'
  //},
  //{
  //  route   : '/lists/create',
  //  name    : 'lists/create',
  //  auth    : true,
  //  moduleId: 'page/todo/create-list',
  //  title   : 'Create list'
  //},
  //{
  //  route   : '/datatable',
  //  name    : 'datatable',
  //  moduleId: 'page/datatable/demo',
  //  nav     : true,
  //  auth    : true,
  //  title   : 'Datatable'
  //},
  //{
  //  route   : '/pager',
  //  name    : 'pager',
  //  moduleId: 'page/pager/demo',
  //  nav     : true,
  //  auth    : true,
  //  title   : 'Pager'
  //},
  //{
  //  route   : '/association-select',
  //  name    : 'association-select',
  //  moduleId: 'page/association-select/demo',
  //  nav     : true,
  //  auth    : true,
  //  title   : 'Association select'
  //},
  //{
  //  route   : '/paged',
  //  name    : 'paged',
  //  moduleId: 'page/paged/demo',
  //  nav     : true,
  //  auth    : true,
  //  title   : 'Paged'
  //},
  //{
  //  route   : '/form',
  //  name    : 'form',
  //  moduleId: 'page/form/demo',
  //  nav     : true,
  //  auth    : true,
  //  title   : 'Form'
  //}
];
