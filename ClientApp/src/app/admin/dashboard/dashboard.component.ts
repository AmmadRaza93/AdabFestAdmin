import { Component, NgModule, ViewChild } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { DashboardService } from 'src/app/_services/dashboard.service';
import { DashboardSummary } from 'src/app/_models/Dashboard';
import { LocalStorageService } from 'src/app/_services/local-storage.service';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { NgbdDatepickerRangePopup } from 'src/app/datepicker-range/datepicker-range-popup';
import { Router } from '@angular/router';
import { AlertService } from '../../_alert/alert.service';

import {
  ApexChart,
  ApexAxisChartSeries,
  ChartComponent,
  ApexDataLabels,
  ApexPlotOptions,
  ApexYAxis,
  ApexLegend,
  ApexGrid,
  ApexStroke,
  ApexTitleSubtitle
} from "ng-apexcharts";
import { SignalrService } from 'src/app/_services/SignalrService';

type ApexXAxis = {
  type?: "category" | "datetime" | "numeric";
  categories?: any;
  labels?: {
    style?: {
      colors?: string | string[];
      fontSize?: string;
    };
  };
};
export type lineChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  dataLabels: ApexDataLabels;
  grid: ApexGrid;
  stroke: ApexStroke;
  title: ApexTitleSubtitle;
};
export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  dataLabels: ApexDataLabels;
  plotOptions: ApexPlotOptions;
  yaxis: ApexYAxis;
  xaxis: ApexXAxis;
  grid: ApexGrid;
  colors: string[];
  legend: ApexLegend;
};
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})

export class DashboardComponent {
  // options = {
  //   autoClose: false,
  //   keepAfterRouteChange: false
  // };
  data$: Observable<DashboardSummary[]>;
  oldData: DashboardSummary[];
  total$: Observable<number>;
  loading$: Observable<boolean>;

  @ViewChild("chart") chart: ChartComponent;
  public chartOptions: Partial<ChartOptions>;
  public lineChartOptions: Partial<lineChartOptions>;

  dashboardSummary = new DashboardSummary();
  async ngOnInit() {
    this.GetDashboard();
    this.GetDashboardMonth();
    this.GetChart();
    await this.singlarService.startConnection();
   
  }

  constructor(public service: DashboardService,
    public ls: LocalStorageService,
    public router: Router,
    private singlarService: SignalrService,
    
    ) {
    this.loading$ = service.loading$;
 
  }
  BindTodaysSales(sales, timeSlot) {
    this.chartOptions = {
      series: [
        {
          name: "Sales",
          data: sales
        }
      ],
      chart: {
        height: 300,
        type: "bar"
      },
      xaxis: {
        categories: timeSlot
      }
    };

  }

  BindMonthData(sales, timeSlot) {
    this.lineChartOptions = {
      series: [
        {
          name: "Booking",
          data: sales
        }
      ],
      chart: {
        height: 350,
        type: "line",
        zoom: {
          enabled: false
        }
      },
      dataLabels: {
        enabled: false
      },
      stroke: {
        curve: "straight"
      },
      title: {
        text: "",
        align: "left"
      },
      grid: {
        row: {
          colors: ["#f3f3f3", "transparent"], // takes an array which will be repeated on columns
          opacity: 0.5
        }
      },
      xaxis: {
        categories: timeSlot
      }
    };

  }
 
  GetDashboard() {
    debugger
    this.service.getAllData().subscribe((res: any) => {
      this.dashboardSummary = res[0];
    });
  }
  GetDashboardMonth() {
    debugger
    this.service.getAllDataMonth().subscribe((res: any) => {
      //this.dashboardSummary = res[0];
      console.log(res);
      this.BindMonthData(res.todaysales.sales,res.todaysales.timeSlot);
    });
  }
 
  GetChart() {
    this.service.getChart().subscribe((res: any) => {
      this.BindTodaysSales(res.todaysales.sales, res.todaysales.timeSlot);
    });
  }
}
 

