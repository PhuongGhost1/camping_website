import { useEffect, useState } from "react";
import {
  OrderItemProps,
  OrderProps,
  ProductFromApi,
  RawOrderItem,
  UserProps,
} from "../App";
import { ApiGateway } from "../services/api/ApiService";

export const useCart = (user: UserProps) => {
  const [carts, setCarts] = useState<OrderItemProps[]>([]);
  const [totalPrice, setTotalPrice] = useState<number>(0);
  const [quantity, setQuantity] = useState(0);
  const [uniqueOrderId, setUniqueOrderId] = useState<string | null>(null);

  const fetchCarts = async () => {
    if (!user) {
      setCarts([]);
      return;
    }

    try {
      const data = await ApiGateway.getOrderByUserId<OrderProps>();
      if (!data?.id) {
        setCarts([]);
        return;
      }

      const orderId = data.id;
      setUniqueOrderId(orderId);

      const orderItems: RawOrderItem[] =
        (await ApiGateway.getAllOrderProducts<RawOrderItem[]>(orderId)) ?? [];

      const orderItemsWithProduct = (
        await Promise.all(
          orderItems.map(async (item) => {
            const product = await ApiGateway.getProductById<ProductFromApi>(
              item.productId
            );
            if (!product) return null;

            return { ...item, product };
          })
        )
      ).filter(Boolean) as OrderItemProps[];

      setCarts(orderItemsWithProduct);
    } catch (error) {
      console.error("Error fetching carts:", error);
      setCarts([]);
    }
  };

  useEffect(() => {
    fetchCarts();
  }, [user]);

  useEffect(() => {
    const total: number = carts.reduce(
      (acc, item) => acc + Number(item.product.price) * (item.quantity || 1),
      0
    );
    const qty = carts.reduce((acc, item) => acc + (item.quantity || 1), 0);

    setTotalPrice(total);
    setQuantity(qty);

    if (user) {
      ApiGateway.updateTotalAmount(total);
    }
  }, [carts, user]);

  return { carts, setCarts, totalPrice, quantity, uniqueOrderId, fetchCarts };
};
