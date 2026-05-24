import { useEffect, useState } from "react";

const defaultForm = {
  title: "",
  description: "",
  status: 1,
  dueDate: "",
};

export function TaskForm({ selectedTask, onSubmit, onCancel }) {
  const [form, setForm] = useState(defaultForm);

  useEffect(() => {
    if (selectedTask) {
      setForm({
        title: selectedTask.title,
        description: selectedTask.description || "",
        status: selectedTask.status,
        dueDate: selectedTask.dueDate
          ? selectedTask.dueDate.substring(0, 10)
          : "",
      });
    } else {
      setForm(defaultForm);
    }
  }, [selectedTask]);

  function handleChange(event) {
    const { name, value } = event.target;

    setForm((current) => ({
      ...current,
      [name]: name === "status" ? Number(value) : value,
    }));
  }

  function handleSubmit(event) {
    event.preventDefault();

    onSubmit({
      ...form,
      dueDate: form.dueDate ? `${form.dueDate}T00:00:00` : null,
    });

    setForm(defaultForm);
  }

  return (
    <form className="task-form" onSubmit={handleSubmit}>
      <h2>{selectedTask ? "Edit task" : "Create task"}</h2>

      <input
        name="title"
        placeholder="Title"
        value={form.title}
        onChange={handleChange}
      />

      <textarea
        name="description"
        placeholder="Description"
        value={form.description}
        onChange={handleChange}
      />

      <div className="form-row">
        <select name="status" value={form.status} onChange={handleChange}>
          <option value={1}>Pending</option>
          <option value={2}>In Progress</option>
          <option value={3}>Done</option>
        </select>

        <input
          name="dueDate"
          type="date"
          value={form.dueDate}
          onChange={handleChange}
        />
      </div>

      <div className="form-actions">
        <button type="submit">
          {selectedTask ? "Save changes" : "Create"}
        </button>

        {selectedTask && (
          <button type="button" className="secondary" onClick={onCancel}>
            Cancel
          </button>
        )}
      </div>
    </form>
  );
}
