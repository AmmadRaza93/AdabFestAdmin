import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { switchMap, tap, map } from 'rxjs/operators';
import { SortColumn, SortDirection } from '../_directives/sortable.directive';
import { State } from '../_models/State';
import { Prescription } from '../_models/Prescription';

interface SearchPrescriptionResult {
  data: Prescription[];
  total: number;
}
const compare = (v1: string, v2: string) => v1 < v2 ? -1 : v1 > v2 ? 1 : 0;
function sort(data: Prescription[], column: SortColumn, direction: string): Prescription[] {
  if (direction === '' || column === '') {
    return data;
  } else {
    return [...data].sort((a, b) => {
      const res = compare(`${a[column]}`, `${b[column]}`);
      return direction === 'asc' ? res : -res;
    });
  }
}
function matches(data: Prescription, term: string) {
  return data.customerName.toLowerCase().includes(term.toLowerCase())
}
@Injectable({
  providedIn: 'root'
})
export class PrescriptionService {
  httpClient: any;
  constructor(private http: HttpClient) {
  }
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _allData$ = new BehaviorSubject<Prescription[]>([]);
  private _data$ = new BehaviorSubject<Prescription[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  public prescription: Prescription[];
  private _state: State = {
    page: 1,
    pageSize: 10,
    searchTerm: '',
    sortColumn: '',
    sortDirection: ''
  };
  get total$() { return this._total$.asObservable(); }
  get loading$() { return this._loading$.asObservable(); }
  get page() { return this._state.page; }
  get pageSize() { return this._state.pageSize; }
  get searchTerm() { return this._state.searchTerm; }

  set page(page: number) { this._set({ page }); }
  set pageSize(pageSize: number) { this._set({ pageSize }); }
  set searchTerm(searchTerm: any) { this._set({ searchTerm }); }
  set sortColumn(sortColumn: SortColumn) { this._set({ sortColumn }); }
  set sortDirection(sortDirection: SortDirection) { this._set({ sortDirection }); }


  get data$() {
    return this._data$.asObservable();
  }
  get allData$() {
    return this._allData$.asObservable();
  }

  getById(id) {
    return this.http.get<Prescription[]>(`api/prescription/prescription/${id}`);
  }
  ExportList(prescriptionID) {
    return this.http.get<Prescription[]>('api/prescription/all/${prescriptionID}');
  }
  getAllData(fromDate, toDate) {

    const url = `api/prescription/all/${fromDate}/${toDate}`;
    console.log(url);
    tap(() => this._loading$.next(true)),
      this.http.get<Prescription[]>(url).subscribe(res => {
        debugger
        this.prescription = res;

        this._data$.next(this.prescription);
        this._allData$.next(this.prescription);

        this._search$.pipe(
          switchMap(() => this._search()),
          tap(() => this._loading$.next(false))
        ).subscribe(result => {
          this._data$.next(result.data);
          this._total$.next(result.total);
        });
        this._search$.next();
      });
  }
  private _set(patch: Partial<State>) {
    Object.assign(this._state, patch);
    this._search$.next();
  }
  private _search(): Observable<SearchPrescriptionResult> {
    const { sortColumn, sortDirection, pageSize, page, searchTerm } = this._state;

    // 1. sort
    let sortedData = sort(this.prescription, sortColumn, sortDirection);

    //// 2. filter
    sortedData = sortedData.filter(data => matches(data, searchTerm));
    const total = sortedData.length;

    // 3. paginate
    const data = sortedData.slice((page - 1) * pageSize, (page - 1) * pageSize + pageSize);
    return of({ data, total });
  }
  clear() {
    // clear by calling subject.next() without parameters
    this._search$.next();
    this._data$.next(null);
    this._allData$.next(null);
    this._total$.next(null);
    this._loading$.next(null);
    this._state = {
      page: 1,
      pageSize: 10,
      searchTerm: '',
      sortColumn: '',
      sortDirection: ''
    };
  }
  insert(data) {
    return this.http.post('api/prescription/insert', data)
      .pipe(map(res => {

        console.log(res);
        return res;
      }));
  }
  update(data) {
    debugger
    return this.http.post('api/prescription/update', data)
      .pipe(map(res => {
        console.log(res);
        return res;
      }));
  }
  delete(data) {
    return this.http.post('api/prescription/delete', data);
  }
}