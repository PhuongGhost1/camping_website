import React from "react";
import { UserProps } from "../../App";
import "./Profile.css";
import { useAuthenContext } from "../../hooks/AuthenContext";

interface ProfileProps {
  user: UserProps | null;
  handleOpenUserInfo: () => void;
  isOpenUserInfo: boolean;
}

const Profile: React.FC<ProfileProps> = ({
  user,
  handleOpenUserInfo,
  isOpenUserInfo,
}) => {
  const { logout } = useAuthenContext();

  return (
    <div className={`user-info-container ${isOpenUserInfo ? "active" : ""}`}>
      <div className="flex-container">
        <div className="user-info-1">
          <div className="name">
            <p>{user?.name}</p>
            <i className="ri-close-line" onClick={handleOpenUserInfo}></i>
          </div>
          <div className="user-info-list">
            <a href="/purchase">Purchase History</a>
            <a href="/profile">Profile Account</a>
            <a href="/wish-lists">Wish lists</a>
          </div>
        </div>
        <div className="user-info-2">
          <a href="/membership/mastercard">
            <span>
              REI Co-op<sup className="link__sup">®</sup> Mastercard
              <sup className="link__sup">®</sup>
            </span>
          </a>
          <div className="user-info-list">
            <span>
              Earn 5% in card rewards on all REI purchases—that’s on top of your
              Co-op Member Reward earned on full-price items. Terms apply.{" "}
              <a href="#">Learn more</a>
            </span>
          </div>
        </div>
        <div className="user-info-3">
          <a href="#">Join REI</a>
          <p>
            Pay once and enjoy a lifetime of benefits, including FREE standard
            shipping, 10% back on eligible purchases and more – just for
            members.
          </p>
          <div className="user-info-list">
            <p>
              <strong>Already a member? </strong>
              <a href="#">Link your membership to your online account</a>
            </p>
          </div>
        </div>
        <a
          href="/"
          onClick={(e) => {
            e.preventDefault();
            logout().then(() => {
              window.location.href = "/";
            });
          }}
        >
          Sign out
        </a>
      </div>
    </div>
  );
};

export default Profile;
