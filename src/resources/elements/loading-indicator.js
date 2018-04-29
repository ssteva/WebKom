import * as nprogress from "nprogress";
import { bindable, noView, inject } from "aurelia-framework";
import { EventAggregator } from "aurelia-event-aggregator";

@noView(["nprogress/nprogress.css"])
@inject(EventAggregator)
export class LoadingIndicator {
  @bindable loading = false;

  constructor(eventAggregator) {
    this.eventAggregator = eventAggregator;
    this.subscription = this.eventAggregator.subscribe("loader", payload => {
      this.loading = payload;
    });
  }
  detached() {
    this.subscription.dispose();
  }
  loadingChanged(newValue) {
    if (newValue) {
      nprogress.start();
    } else {
      //nprogress.done();
    }
  }
}
