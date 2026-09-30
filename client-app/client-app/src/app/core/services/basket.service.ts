export interface BasketItem {
  productId: number;
  name: string;
  pictureUrl: string;
  brand: string;
  unitPrice: number;
  quantity: number;
  lineTotal: number;
}

export interface Basket {
  key: string;
  items: BasketItem[];
  totalItems: number;
  subtotal: number;
}
