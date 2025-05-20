import React, { useRef, useState } from "react";
import "./OtpInput.css";
import { ApiGateway } from "../../../services/api/ApiService";

interface OtpInputProps {
  ChangeStateType: (type: string) => void;
  onRegisterSuccess?: () => void;
  emailInput?: string;
}

const OtpInput: React.FC<OtpInputProps> = ({
  ChangeStateType,
  onRegisterSuccess,
  emailInput,
}) => {
  const [otp, setOtp] = useState<string[]>(new Array(6).fill(""));
  const inputsRef = useRef<Array<HTMLInputElement | null>>([]);
  const [isFilled, setIsFilled] = useState(false);

  const handleChange = async (value: string, index: number) => {
    if (!/^[0-9]?$/.test(value)) return;

    const newOtp = [...otp];
    newOtp[index] = value;
    setOtp(newOtp);

    if (value) {
      if (index < 5) {
        inputsRef.current[index + 1]?.focus();
      }

      const isAllFilled = newOtp.every((digit) => digit !== "");
      const isLastIndex = index === 5;

      if (isAllFilled && isLastIndex) {
        if (!emailInput) {
          console.error("Email input is missing.");
          return;
        }

        setIsFilled(true);

        try {
          const isSuccess = await ApiGateway.VerifyOTP(
            newOtp.join(""),
            emailInput
          );

          if (isSuccess) {
            ChangeStateType("Login");

            if (onRegisterSuccess) {
              onRegisterSuccess();
            }
          } else {
            console.error("Invalid OTP");
          }
        } catch (error) {
          console.error("Error verifying OTP:", error);
        } finally {
          setOtp(new Array(6).fill(""));
          setIsFilled(false);
        }
      }
    }
  };

  const handleKeyDown = (
    e: React.KeyboardEvent<HTMLInputElement>,
    index: number
  ) => {
    if (e.key === "ArrowLeft" && index > 0) {
      inputsRef.current[index - 1]?.focus();
    }
    if (e.key === "ArrowRight" && index < 5) {
      inputsRef.current[index + 1]?.focus();
    }
  };

  const handlePaste = (e: React.ClipboardEvent<HTMLInputElement>) => {
    e.preventDefault();
    const pastedData = e.clipboardData.getData("text/plain").split("");
    if (pastedData.length === 6) {
      const newOtp = [...otp];
      for (let i = 0; i < 6; i++) {
        newOtp[i] = pastedData[i] || "";
      }
      setOtp(newOtp);
      newOtp.forEach((_, index) => {
        inputsRef.current[index]?.focus();
      });
    }
  };

  return (
    <>
      <p className="sign-up space-mg">
        <span className="bold" onClick={() => ChangeStateType("Login")}>
          Back To Login
        </span>
      </p>
      <p className="welcomeback">Verify OTP</p>
      <p className="welcome-description">
        Enter the OTP code sent to your email address
      </p>

      <div style={{ display: "flex", gap: "10px", justifyContent: "center" }}>
        {otp.map((digit, index) => (
          <input
            key={index}
            ref={(el) => {
              inputsRef.current[index] = el;
            }}
            type="text"
            maxLength={1}
            value={digit}
            onPaste={handlePaste}
            onFocus={(e) => e.target.select()}
            onChange={(e) => handleChange(e.target.value, index)}
            onKeyDown={(e) => handleKeyDown(e, index)}
            style={{
              width: "50px",
              height: "50px",
              fontSize: "20px",
              textAlign: "center",
              border: "1px solid #ccc",
              borderRadius: "6px",
              marginBottom: "20px",
              cursor: isFilled ? "not-allowed" : "text",
            }}
            disabled={isFilled}
          />
        ))}
      </div>

      <button className="verify-btn">Verify</button>
      <p className="sign-up">
        Didn't receive the code?{" "}
        <span className="bold" onClick={() => ChangeStateType("Resend")}>
          Resend
        </span>
      </p>
    </>
  );
};

export default OtpInput;
