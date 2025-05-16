import React from "react";
import "./CustomToast.css"; // Import CSS thuần

interface CustomToastProps {
  emoji: string;
  message: string;
}

const CustomToast: React.FC<CustomToastProps> = ({ emoji, message }) => {
  return (
    <div className="custom-toast">
      <span className="emoji">{emoji}</span>
      <span className="message">{message}</span>
    </div>
  );
};

export default CustomToast;
