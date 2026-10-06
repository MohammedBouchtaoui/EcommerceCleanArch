export type OrderStatus = 'Pending' | 'Paid' | 'Shipped' | 'Delivered' | 'Cancelled';

export const STATUS_LABELS: Record<OrderStatus, string> = {
  Pending: 'En attente de paiement',
  Paid: 'Payée',
  Shipped: 'Expédiée',
  Delivered: 'Livrée',
  Cancelled: 'Annulée',
};

export interface ShippingAddress {
  fullName: string;
  line1: string;
  city: string;
  postalCode: string;
  country: string;
}

export interface OrderItem {
  productId: number;
  productName: string;
  pictureUrl: string;
  unitPrice: number;
  quantity: number;
  lineTotal: number;
}
















export interface Order {
  id: number;
  createdAtUtc: string;
  status: OrderStatus;
  shippingAddress: ShippingAddress;
  items: OrderItem[];
  subtotal: number;
  shippingCost: number;
  total: number;
}

export interface CreateOrderRequest {
  basketKey: string;
  shippingAddress: ShippingAddress;
}
