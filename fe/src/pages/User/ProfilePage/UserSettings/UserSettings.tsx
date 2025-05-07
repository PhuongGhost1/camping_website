import { useEffect, useState } from "react";
import "./UserSettings.css";

const UserSettings = () => {
  const [isOpenEditAccount, setIsOpenEditAccount] = useState(false);
  const [firstName, setFirstName] = useState("Hoang");
  const [lastName, setLastName] = useState("Trong Phuong");
  const [email, setEmail] = useState("trngphng94@gmail.com");
  const [phone, setPhone] = useState("");
  const [currentPwd, setCurrentPwd] = useState("");
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
    const allFieldsFilled =
      firstName.trim() !== "" &&
      lastName.trim() !== "" &&
      email.trim() !== "" &&
      phone.trim() !== "";

    const passwordFieldsFilled =
      currentPwd.trim() !== "" &&
      newPwd.trim() !== "" &&
      reEnterPwd.trim() !== "";

    setIsAllFieldsFilled(allFieldsFilled);
    setIsPasswordFieldsFilled(passwordFieldsFilled);
  }, [firstName, lastName, email, phone, currentPwd, newPwd, reEnterPwd]);

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
                type="email"
                id="email"
                name="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
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
              />
            </div>
          </div>
          <div className="edit-button-settings">
            <button
              disabled={!isAllFieldsFilled}
              className={`${isAllFieldsFilled ? "" : "disabled"}`}
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
            <p>Phuong Hoang</p>
          </div>
          <div className="settings-option">
            <label htmlFor="email">
              <strong>Email</strong>
            </label>
            <p>trngphng94@gmail.com</p>
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
                  className={`input-pwd ${isOpenChangePassword ? "" : "open"}`}
                />
                <div
                  className={`change-pwd ${isOpenChangePassword ? "open" : ""}`}
                >
                  <div className="edit-settings-option">
                    <div className="edit-setting">
                      <label htmlFor="current-password">
                        Current password
                        <sup>*</sup>
                      </label>
                      <input
                        type="password"
                        id="current-password"
                        name="current-password"
                        value={currentPwd}
                        onChange={(e) => setCurrentPwd(e.target.value)}
                      />
                    </div>
                  </div>
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
