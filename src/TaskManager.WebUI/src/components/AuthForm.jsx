import { useState } from "react";
import { api, setToken } from "../api/apiClient";

export function AuthForm({ onAuthenticated }) {
  const [mode, setMode] = useState("login");
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  async function handleSubmit(event) {
    event.preventDefault();
    setError("");

    try {
      if (mode === "register") {
        await api.register({ username, password });
      }

      const result = await api.login({ username, password });

      setToken(result.token);
      onAuthenticated(result.username || username);
    } catch (err) {
      setError(err.message);
    }
  }

  return (
    <div className="auth-card">
      <h1>Task Manager</h1>
      <p>Manage your personal tasks.</p>

      <form onSubmit={handleSubmit}>
        <label>Username</label>
        <input value={username} onChange={(e) => setUsername(e.target.value)} />

        <label>Password</label>
        <input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />

        {error && <div className="error">{error}</div>}

        <button type="submit">{mode === "login" ? "Login" : "Register"}</button>
      </form>

      <button
        className="link-button"
        onClick={() => setMode(mode === "login" ? "register" : "login")}
      >
        {mode === "login"
          ? "Create an account"
          : "Already have an account? Login"}
      </button>
    </div>
  );
}
