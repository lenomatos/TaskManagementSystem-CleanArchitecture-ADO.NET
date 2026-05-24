const statusClass = {
  1: "pending",
  2: "progress",
  3: "done",
};

export function TaskList({ tasks, onEdit, onDelete }) {
  if (tasks.length === 0) {
    return <p className="empty">No tasks found.</p>;
  }

  return (
    <div className="task-grid">
      {tasks.map((task) => (
        <div className="task-card" key={task.id}>
          <div className="task-header">
            <h3>{task.title}</h3>
            <span className={`badge ${statusClass[task.status]}`}>
              {task.statusName}
            </span>
          </div>

          <p>{task.description || "No description"}</p>

          <small>
            Due:{" "}
            {task.dueDate
              ? new Date(task.dueDate).toLocaleDateString()
              : "No due date"}
          </small>

          <div className="task-actions">
            <button onClick={() => onEdit(task)}>Edit</button>
            <button className="danger" onClick={() => onDelete(task)}>
              Delete
            </button>
          </div>
        </div>
      ))}
    </div>
  );
}
