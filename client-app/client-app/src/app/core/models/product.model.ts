export interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  pictureUrl: string;
  brand: string;
  category: string;
  stockQuantity: number;
}

export interface Pagination<T> {
  pageIndex: number;
  pageSize: number;
  count: number;
  data: T[];
}

export interface ProductParams {
  pageIndex: number;
  pageSize: number;
  search: string;
  brand: string;
  category: string;
  sort: string;
}
