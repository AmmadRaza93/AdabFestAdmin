import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { switchMap, tap, map } from 'rxjs/operators';
import { SortColumn, SortDirection } from '../_directives/sortable.directive';
import { State } from '../_models/State';
import { User } from '../_models/User';
import { Permission, PermissionForms } from '../_models/Permission';


interface SearchUsersResult {
  data: User[];
  //obj: PermissionForms[];
  total: number;
}
const compare = (v1: string, v2: string) => v1 < v2 ? -1 : v1 > v2 ? 1 : 0;

function sort(data: User[], column: SortColumn, direction: string): User[] {
  if (direction === '' || column === '') {
    return data;
  } else {
    return [...data].sort((a, b) => {
      const res = compare(`${a[column]}`, `${b[column]}`);
      return direction === 'asc' ? res : -res;
    });
  }
}

function matches(data: User, term: string) {

  return data.userName.toLowerCase().includes(term.toLowerCase())
}

@Injectable({
  providedIn: 'root'
})

export class UserService {
  public userModel: User;
  constructor(private http: HttpClient) {
  }

  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _allData$ = new BehaviorSubject<User[]>([]);
  private _data$ = new BehaviorSubject<User[]>([]);
  private _obj$ = new BehaviorSubject<PermissionForms[]>([]);
  private _allObj$ = new BehaviorSubject<PermissionForms[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  public user: User[];
  public permission: PermissionForms[];
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
  get obj$() {
    return this._obj$.asObservable();
  }
  get allData$() {
    return this._allData$.asObservable();
  }
  get allObj$() {
    return this._allObj$.asObservable();
  }

  loadUser() {
    return this.http.get<Permission[]>(`api/user/permission`);
  }

  ExportList() {
    return this.http.get<User[]>(`api/user/getall`);
  }
  getById(id) {
    return this.http.get<User[]>(`api/user/${id}`);
  }
  getAllData() {
    const url = `api/user/getall`;
    console.log(url);
    tap(() => this._loading$.next(true)),
      this.http.get<User[]>(url).subscribe(res => {
        this.user = res;

        this._data$.next(this.user);
        this._allData$.next(this.user);
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

  private _search(): Observable<SearchUsersResult> {

    const { sortColumn, sortDirection, pageSize, page, searchTerm } = this._state;
    // 1. sort
    let sortedData = sort(this.user, sortColumn, sortDirection);
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
    debugger
    return this.http.post(`api/user/insertuser`, data)
      .pipe(map(res => {
        console.log(res);
        return res;
      }));
  }

  update(updateData) {
    return this.http.post(`api/user/updateuser`, updateData)
      .pipe(map(res => {
        console.log(res);
        return res;
      }));
  }
  delete(data) {
    return this.http.post(`api/user/deleteuser`, data);
  }

  insertpermission(obj) {
    debugger
    return this.http.post(`api/user/insert/permission`, obj)
      .pipe(map(obj => {
        console.log(obj);
        return obj;
      }));
  }
  permissionupdate(updateobj) {
    debugger
    return this.http.post(`api/user/update/permission`, updateobj)
      .pipe(map(updateobj => {
        console.log(updateobj);
        return updateobj;
      }));
  }
  getPermissionId(id) {
    return this.http.get<PermissionForms>(`api/user/userpermission/${id}`);
  }
  getpermission() {
    debugger
    const url = `api/user/allpermission`;
    console.log(url);
    tap(() => this._loading$.next(true)),
      this.http.get<PermissionForms[]>(url).subscribe(res => {
        this.permission = res;

        this._obj$.next(this.permission);
        this._allObj$.next(this.permission);
        this._search$.next();
      });
  }
  statusUpdate(data) {
    debugger
    return this.http.post('api/user/status', data);
  }
}
