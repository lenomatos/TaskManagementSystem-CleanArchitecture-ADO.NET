import { useEffect, useState } from "react";
import { AuthForm } from "./components/AuthForm";
import { TaskForm } from "./components/TaskForm";
import { TaskList } from "./components/TaskList";
import { ConfirmModal } from "./components/ConfirmModal";
import { api, clearToken, getToken } from "./api/apiClient";
import "./App.css";

export default function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(Boolean(getToken()));
  const [username, setUsername] = useState("");
  const [tasks, setTasks] = useState([]);
  const [selectedTask, setSelectedTask] = useState(null);
  const [error, setError] = useState("");
  const [taskToDelete, setTaskToDelete] = useState(null);

  async function loadTasks() {
    try {
      setError("");
      const data = await api.getTasks();
      setTasks(data);
    } catch (err) {
      setError(err.message);
    }
  }

  useEffect(() => {
    if (isAuthenticated) {
      loadTasks();
    }
  }, [isAuthenticated]);

  async function handleSubmitTask(taskData) {
    try {
      if (selectedTask) {
        await api.updateTask(selectedTask.id, taskData);
      } else {
        await api.createTask(taskData);
      }

      setSelectedTask(null);
      await loadTasks();
    } catch (err) {
      setError(err.message);
    }
  }

  async function handleDeleteTask() {
    if (!taskToDelete) return;

    try {
      await api.deleteTask(taskToDelete.id);
      setTaskToDelete(null);
      await loadTasks();
    } catch (err) {
      setError(err.message);
    }
  }

  function handleLogout() {
    clearToken();
    setIsAuthenticated(false);
    setTasks([]);
    setUsername("");
  }

  if (!isAuthenticated) {
    return (
      <AuthForm
        onAuthenticated={(name) => {
          setUsername(name);
          setIsAuthenticated(true);
        }}
      />
    );
  }

  return (
    <main className="app">
      <header className="topbar">
        <div>
          <h1>Task Manager</h1>
          <p>Welcome {username || "back"}</p>
        </div>

        <button className="secondary" onClick={handleLogout}>
          Logout
        </button>
      </header>

      {error && <div className="error">{error}</div>}

      <section className="layout">
        <TaskForm
          selectedTask={selectedTask}
          onSubmit={handleSubmitTask}
          onCancel={() => setSelectedTask(null)}
        />

        <TaskList
          tasks={tasks}
          onEdit={setSelectedTask}
          onDelete={setTaskToDelete}
        />
      </section>

      {taskToDelete && (
        <ConfirmModal
          title="Delete task"
          message={`Are you sure you want to delete "${taskToDelete.title}"?`}
          onCancel={() => setTaskToDelete(null)}
          onConfirm={handleDeleteTask}
        />
      )}
    </main>
  );
}
