import React, { useEffect, useState } from "react";
import "./UserSettings.css";
import { IActionResult, UserProps } from "../../../../App";
import { ApiGateway } from "../../../../services/api/ApiService";
import { toast } from "react-toastify";
import CustomToast from "../../../../helper/toasts/CustomToast";
interface UserSettingsProps {
  user: UserProps | null;
  loadUser: () => Promise<void>;
}

const UserSettings: React.FC<UserSettingsProps> = ({ user, loadUser }) => {
  const [isOpenEditAccount, setIsOpenEditAccount] = useState(false);
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [phone, setPhone] = useState("0123456789");
  const [newPwd, setNewPwd] = useState("");
  const [reEnterPwd, setReEnterPwd] = useState("");
  const [isOpenChangePassword, setIsOpenChangePassword] = useState(false);
  const [isAllFieldsFilled, setIsAllFieldsFilled] = useState(false);
  const [isPasswordFieldsFilled, setIsPasswordFieldsFilled] = useState(false);

  const openEditAccount = () => {
    setIsOpenEditAccount(true);
  };

  const closeEditAccount = () => {
    setIsOpenEditAccount(false);
  };

  const openChangePassword = () => {
    setIsOpenChangePassword(true);
  };

  const closeChangePassword = () => {
    setIsOpenChangePassword(false);
  };

  useEffect(() => {
    if (user?.name) {
      const nameParts = user.name.trim().split(" ");
      setFirstName(nameParts[0]);
      setLastName(nameParts.slice(1).join(" "));
    }
  }, [user]);

  useEffect(() => {
    const allFieldsFilled =
      firstName.trim() !== "" && lastName.trim() !== "" && phone.trim() !== "";

    const passwordFieldsFilled =
      newPwd.trim() !== "" && reEnterPwd.trim() !== "";

    setIsAllFieldsFilled(allFieldsFilled);
    setIsPasswordFieldsFilled(passwordFieldsFilled);
  }, [firstName, lastName, phone, newPwd, reEnterPwd]);

  const handleUpdateAccount = async () => {
    try {
      const data = await ApiGateway.UpdateUserInfo<IActionResult>(
        firstName,
        lastName
      );

      if (data?.status === 200) {
        toast.success(
          <CustomToast emoji="✅" message="Account updated successfully!" />,
          {
            position: "top-right",
            autoClose: 4000,
            pauseOnHover: true,
            draggable: true,
            theme: "colored",
          }
        );
        setIsOpenEditAccount(false);
        await loadUser();
      } else if (data?.status === 404) {
        toast.error(<CustomToast emoji="❌" message="Account not found!" />, {
          position: "top-right",
          autoClose: 4000,
          pauseOnHover: true,
          draggable: true,
          theme: "colored",
        });
      } else {
        toast.error(
          <CustomToast emoji="❌" message="Error while updating account!" />,
          {
            position: "top-right",
            autoClose: 4000,
            pauseOnHover: true,
            draggable: true,
            theme: "colored",
          }
        );
      }
    } catch (error) {
      console.error(error);
      toast.error(
        <CustomToast emoji="❌" message="Error while updating account!" />,
        {
          position: "top-right",
          autoClose: 4000,
          pauseOnHover: true,
          draggable: true,
          theme: "colored",
        }
      );
    }
  };

  const handleUpdatePassword = async () => {
    if (newPwd !== reEnterPwd) {
      toast.error(
        <CustomToast emoji="❌" message="Passwords do not match!" />,
        {
          position: "top-right",
          autoClose: 4000,
          pauseOnHover: true,
          draggable: true,
          theme: "colored",
        }
      );
      return;
    }

    try {
      const data = await ApiGateway.UpdateUserPassword<IActionResult>(
        newPwd,
        reEnterPwd
      );

      if (data?.status === 200) {
        toast.success(
          <CustomToast emoji="✅" message="Password updated successfully!" />,
          {
            position: "top-right",
            autoClose: 4000,
            pauseOnHover: true,
            draggable: true,
            theme: "colored",
          }
        );
        setIsOpenChangePassword(false);
        await loadUser();
      } else if (data?.status === 404) {
        toast.error(<CustomToast emoji="❌" message="Account not found!" />, {
          position: "top-right",
          autoClose: 4000,
          pauseOnHover: true,
          draggable: true,
          theme: "colored",
        });
      } else {
        toast.error(
          <CustomToast emoji="❌" message="Error while updating password!" />,
          {
            position: "top-right",
            autoClose: 4000,
            pauseOnHover: true,
            draggable: true,
            theme: "colored",
          }
        );
      }
    } catch (error) {
      console.error(error);
      toast.error(
        <CustomToast emoji="❌" message="Error while updating password!" />,
        {
          position: "top-right",
          autoClose: 4000,
          pauseOnHover: true,
          draggable: true,
          theme: "colored",
        }
      );
    }
  };

  return (
    <div className="user-settings">
      <h2>
        {isOpenEditAccount ? "Edit account settings" : "Account Settings"}
      </h2>
      {isOpenEditAccount ? (
        <>
          <div className="edit-settings-option">
            <div className="edit-setting">
              <label htmlFor="firstname">
                First Name
                <sup>*</sup>
              </label>
              <input
                type="text"
                id="firstname"
                name="firstname"
                value={firstName}
                onChange={(e) => setFirstName(e.target.value)}
              />
            </div>
            <div className="edit-setting">
              <label htmlFor="lastname">
                Last Name
                <sup>*</sup>
              </label>
              <input
                type="lastname"
                id="lastname"
                name="lastname"
                value={lastName}
                onChange={(e) => setLastName(e.target.value)}
              />
            </div>
          </div>
          <div className="edit-settings-option">
            <div className="edit-setting">
              <label htmlFor="email">
                Email
                <sup>*</sup>
              </label>
              <input
                disabled={true}
                type="email"
                id="email"
                name="email"
                value={user?.email}
                style={{ cursor: "not-allowed" }}
              />
            </div>
          </div>
          <div className="edit-settings-option">
            <div className="edit-setting">
              <label htmlFor="phone">
                Phone Number
                <sup>*</sup>
              </label>
              <input
                type="tel"
                id="phone"
                name="phone"
                value={phone}
                onChange={(e) => setPhone(e.target.value)}
                pattern="[0-9]{3}-[0-9]{3}-[0-9]{4}"
                placeholder="123-456-7890"
                style={{ cursor: "not-allowed" }}
                disabled={true}
              />
            </div>
          </div>
          <div className="edit-button-settings">
            <button
              disabled={!isAllFieldsFilled}
              className={`${isAllFieldsFilled ? "" : "disabled"}`}
              onClick={handleUpdateAccount}
            >
              Save
            </button>
            <button onClick={closeEditAccount}>Cancel</button>
          </div>
        </>
      ) : (
        <>
          <div className="settings-option">
            <label htmlFor="username">
              <strong>Name</strong>
            </label>
            <p>{user?.name}</p>
          </div>
          <div className="settings-option">
            <label htmlFor="email">
              <strong>Email</strong>
            </label>
            <p>{user?.email}</p>
          </div>
          <div className="button-settings" onClick={openEditAccount}>
            <i className="ri-pencil-line"></i>
            <p>Edit Account Settings</p>
          </div>
          <div className="settings-option">
            <div className="label-pwd">
              <label htmlFor="password">
                <strong>Password</strong>
              </label>
              <>
                <input
                  type="password"
                  id="password"
                  name="password"
                  value={123456789}
                  readOnly={true}
                  className={`input-pwd ${isOpenChangePassword ? "" : "open"}`}
                  style={{ cursor: "not-allowed" }}
                />
                <div
                  className={`change-pwd ${isOpenChangePassword ? "open" : ""}`}
                >
                  <div className="edit-settings-option">
                    <div className="edit-setting">
                      <label htmlFor="new-password">
                        New password
                        <sup>*</sup>
                      </label>
                      <input
                        type="password"
                        id="new-password"
                        name="new-password"
                        value={newPwd}
                        onChange={(e) => setNewPwd(e.target.value)}
                      />
                    </div>
                  </div>
                  <div className="edit-settings-option">
                    <div className="edit-setting">
                      <label htmlFor="re-enter-password">
                        Re-enter password
                        <sup>*</sup>
                      </label>
                      <input
                        type="password"
                        id="re-enter-password"
                        name="re-enter-password"
                        value={reEnterPwd}
                        onChange={(e) => setReEnterPwd(e.target.value)}
                      />
                    </div>
                  </div>
                  <div className="edit-button-settings">
                    <button
                      disabled={!isPasswordFieldsFilled}
                      className={`${isPasswordFieldsFilled ? "" : "disabled"}`}
                      onClick={handleUpdatePassword}
                    >
                      Save
                    </button>
                    <button onClick={closeChangePassword}>Cancel</button>
                  </div>
                </div>
              </>
            </div>
            {!isOpenChangePassword && (
              <button onClick={openChangePassword}>Change Password</button>
            )}
          </div>
        </>
      )}
    </div>
  );
};

export default UserSettings;
