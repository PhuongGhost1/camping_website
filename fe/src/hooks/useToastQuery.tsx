import { useEffect, useRef } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import { ApiGateway } from "../services/api/ApiService";
import CustomToast from "../helper/toasts/CustomToast";

const useToastQuery = () => {
  const location = useLocation();
  const navigate = useNavigate();

  const hasHandled = useRef(false);

  useEffect(() => {
    if (hasHandled.current) return;

    const query = new URLSearchParams(location.search);
    const paymentStatus = query.get("payment");
    const paymentId = query.get("paymentId");
    const payerId = query.get("PayerID");
    const token = query.get("token");

    const clearQuery = () => {
      query.delete("payment");
      query.delete("paymentId");
      query.delete("PayerID");
      query.delete("token");

      navigate(
        { pathname: location.pathname, search: query.toString() },
        { replace: true }
      );
    };

    const confirmPayment = async () => {
      hasHandled.current = true;
      if (!paymentId || !payerId || !token) {
        toast.error("⚠️ Missing payment confirmation parameters.");
        clearQuery();
        return;
      }

      try {
        const result = await ApiGateway.confirmPayment(
          paymentId,
          payerId,
          token
        );

        if (result) {
          toast.success(
            <CustomToast emoji="🎉" message="Payment Successfully!" />,
            {
              position: "top-right",
              autoClose: 4000,
              pauseOnHover: true,
              draggable: true,
              theme: "colored",
            }
          );
        } else {
          toast.error(
            <CustomToast emoji="❌" message="Payment was not approved." />,
            {
              position: "top-right",
              autoClose: 4000,
              pauseOnHover: true,
              draggable: true,
              theme: "colored",
            }
          );
        }
      } catch {
        toast.error(
          <CustomToast emoji="❌" message="Error while confirming payment." />,
          {
            position: "top-right",
            autoClose: 4000,
            pauseOnHover: true,
            draggable: true,
            theme: "colored",
          }
        );
      }

      clearQuery();
    };

    if (paymentStatus === "success") {
      confirmPayment();
    }

    if (paymentStatus === "fail") {
      hasHandled.current = true;
      toast.error(
        <CustomToast emoji="❌" message="Payment Failed. Please Try Again!" />,
        {
          position: "top-right",
          autoClose: 4000,
          pauseOnHover: true,
          draggable: true,
          theme: "colored",
        }
      );
      clearQuery();
    }
  }, [location, navigate]);
};

export default useToastQuery;
