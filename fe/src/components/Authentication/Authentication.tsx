import React, { useState } from "react";
import "./Authentication.css";
import LoginSide from "../../assets/login-side.png";
import { useAuthenContext } from "../../hooks/AuthenContext";
import { ApiGateway } from "../../services/api/ApiService";
import { toast } from "react-toastify";

interface AuthenticationProps {
  closeLogin: () => void;
  type: string;
}

const Authentication: React.FC<AuthenticationProps> = ({
  closeLogin,
  type,
}) => {
  const [emailInput, setEmailInput] = useState<string>("");
  const [passwordInput, setPasswordInput] = useState<string>("");
  const [usernameInput, setUsernameInput] = useState<string>("");
  const [isWrongSignIn, setIsWrongSignIn] = useState<boolean>(false);
  const [typeInput, setTypeInput] = useState<string>(type);
  const { login } = useAuthenContext();

  const onChangeEmail = (e: React.ChangeEvent<HTMLInputElement>) => {
    setEmailInput(e.target.value);
  };

  const onChangePassword = (e: React.ChangeEvent<HTMLInputElement>) => {
    setPasswordInput(e.target.value);
  };

  const onChangeUserName = (e: React.ChangeEvent<HTMLInputElement>) => {
    setUsernameInput(e.target.value);
  };

  const onClickSignIn = async () => {
    const isSuccess = await login(emailInput, passwordInput);
    if (isSuccess) {
      closeLogin();
      handleLoginSuccess();
    } else {
      setIsWrongSignIn(true);
    }
  };

  const onClickRegister = async () => {
    const isSuccess = await ApiGateway.Register(
      emailInput,
      passwordInput,
      usernameInput
    );
    if (isSuccess) {
      setTypeInput("Login");
      handleRegisterSuccess();
    } else {
      handleRegisterFail();
    }
  };

  const handleLoginSuccess = () => {
    toast.success("Login Successfully");
  };

  const handleRegisterSuccess = () => {
    toast.success("Welcome To Camping WebSite");
  };

  const handleRegisterFail = () => {
    toast.error("Email already exists");
  };

  const ChangeStateType = (value: string) => {
    setTypeInput(value);
  };

  return (
    <div id="login">
      <div className="background" onClick={closeLogin}></div>
      <div className="login-form">
        <div className="form-side">
          <div className="form-center">
            {typeInput === "Login" && (
              <>
                <p className="welcomeback">
                  Holla, <br />
                  Welcome Back
                </p>
                <p className="welcome-description">
                  Hey, welcome back to your special place
                </p>
                <input
                  type="text"
                  className="email input"
                  placeholder="Email"
                  value={emailInput}
                  onChange={onChangeEmail}
                />
                <input
                  type="password"
                  className="password input"
                  placeholder="Password"
                  value={passwordInput}
                  onChange={onChangePassword}
                />
                <div className="remember-forget">
                  {isWrongSignIn && (
                    <p className="wrong-txt">
                      Incorrect username/email or password
                    </p>
                  )}
                  <div className="remeber">
                    <input type="checkbox" className="check-rem" />
                    <span className="rem-span">Remember me</span>
                  </div>
                  <a
                    className="forgot"
                    onClick={() => ChangeStateType("Forgot")}
                  >
                    Forgot Password?
                  </a>
                </div>
                <button className="sign-in" onClick={onClickSignIn}>
                  Sign In
                </button>
                <p className="sign-up">
                  Don't have an account?{" "}
                  <span
                    className="bold"
                    onClick={() => ChangeStateType("SignUp")}
                  >
                    Sign Up
                  </span>
                </p>
              </>
            )}
            {typeInput === "SignUp" && (
              <>
                <p className="welcomeback">
                  Create <br />
                  new account
                </p>
                <p className="welcome-description">
                  Enter your details to create your account and get started
                </p>
                <input
                  type="text"
                  className="username input"
                  placeholder="Username"
                  value={usernameInput}
                  onChange={onChangeUserName}
                />
                <input
                  type="text"
                  className="email input"
                  placeholder="Email"
                  value={emailInput}
                  onChange={onChangeEmail}
                />
                <input
                  type="password"
                  className="password input"
                  placeholder="Password"
                  value={passwordInput}
                  onChange={onChangePassword}
                />
                <button className="sign-in" onClick={onClickRegister}>
                  Sign Up
                </button>
                <p className="sign-up">
                  Already have an account{" "}
                  <span
                    className="bold"
                    onClick={() => ChangeStateType("Login")}
                  >
                    Login
                  </span>
                </p>
              </>
            )}
            {typeInput === "Forgot" && (
              <>
                <p className="welcomeback forgot-title">Forgot Password?</p>
                <p className="welcome-description forgot-des">
                  Enter the email address you used when you joined and we’ll
                  send you instructions to reset your password.
                  <br />
                  <br />
                  For security reasons, we do NOT store your password. So rest
                  assured that we will never send your password via email.
                </p>
                <input
                  type="text"
                  className="email-forgot input"
                  placeholder="Email Address"
                  value={emailInput}
                  onChange={onChangeEmail}
                />
                <button className="sign-in">Send Reset</button>
                <p className="sign-up">
                  Remember Password?{" "}
                  <span
                    className="bold"
                    onClick={() => ChangeStateType("Login")}
                  >
                    Login
                  </span>
                </p>
              </>
            )}
          </div>
        </div>
        <div className="side-image">
          <img src={LoginSide} alt="Image" />
        </div>
      </div>
    </div>
  );
};

export default Authentication;
