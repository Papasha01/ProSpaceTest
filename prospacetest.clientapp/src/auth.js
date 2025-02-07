// src/auth.js
export const auth = {
  isAuthenticated: false,
  userRole: null,

  login(role) {
    this.isAuthenticated = true;
    this.userRole = role;
  },

  logout() {
    this.isAuthenticated = false;
    this.userRole = null;
  },

  hasRole(role) {
    return this.isAuthenticated && this.userRole === role;
  },
};
