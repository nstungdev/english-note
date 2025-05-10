export interface Paging<T = any> {
  items: T[]; // Array of items of type T
  totalCount: number; // Total number of items
  page: number; // Current page number
  pageSize: number; // Number of items per page
  totalPages: number; // Total number of pages
}
