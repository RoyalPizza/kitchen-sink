/** @type {{name: string, description: string }[]} */
let tasks = [];

function fetchTasks() {

    fetch('http://localhost:3000/tasks')
        .then(response => response.json())
        .then(data => {
            tasks = data;
            buildTable();
        })
        .catch(error => console.error('Error:', error));

    buildTable();
}

function buildTable() {
    const tableBody = document.getElementById("taskTableBody");
    tableBody.innerHTML = "";
    tasks.forEach((task, index) => {
        const row = document.createElement("tr");
        const nameCell = document.createElement("td");
        nameCell.addEventListener("click", () => cellClick(nameCell, index, "name"));
        const descriptionCell = document.createElement("td");
        nameCell.textContent = task.name;
        descriptionCell.textContent = task.description;
        row.appendChild(nameCell);
        row.appendChild(descriptionCell);
        tableBody.appendChild(row);
    });

    document.getElementById("taskCount").textContent = tasks.length;
}

/**
 * @param {HTMLElement} cell - The clicked table cell
 * @param {number} taskIndex - Index of the task in the tasks array
 * @param {string} property - Task property to update ("name" or "description")
 */
function cellClick(cell, taskIndex, property) {
    if (cell.firstElementChild && cell.firstElementChild.tagName == "INPUT") {
        return;
    }

    const input = document.createElement("input");
    input.type = "text";
    input.value = cell.textContent;

    cell.innerHTML = "";
    cell.appendChild(input);
    input.focus();

    const debouncedValidate = debounce(() => validateCell(cell, input, taskIndex, property), 1500);
    input.oninput = debouncedValidate;
    input.onblur = () => cellFinishedEdit(cell, input, taskIndex, property, debouncedValidate);
    input.onkeydown = (e) => {
        if (e.key == "Enter") cellFinishedEdit(cell, input, taskIndex, property, debouncedValidate);
    }
}

/**
 * @param {HTMLElement} cell - The clicked table cell
 * @param {HTMLElement} input - The input text box of the cell
 * @param {number} taskIndex - Index of the task in the tasks array
 * @param {string} property - Task property to update ("name" or "description")
 * @param {Function} debouncedValidate - Debounced validation function
 */
function cellFinishedEdit(cell, input, taskIndex, property, debouncedValidate) {
    const value = input.value;
    const validation = validateCell(cell, input, taskIndex, property);
    if (validation.isValid) {
        debouncedValidate.cancel();
        cell.innerHTML = "";
        cell.textContent = value;
        tasks[taskIndex][property] = value;
    }
}

/**
 * @param {HTMLElement} cell - The clicked table cell
 * @param {HTMLElement} input - The input text box of the cell
 * @param {number} taskIndex - Index of the task in the tasks array
 * @param {string} property - Task property to update ("name" or "description")
 * @returns {{ isValid: boolean, errorMessage: string }} Validation result
 */
function validateCell(cell, input, taskIndex, property) {

    /** @type {{isValid: boolean, errorMessage: string }} */
    let validation = { isValid: true, errorMessage: "" };

    if (property == "name") {
        if (tasks.some((task, index) => index != taskIndex && task.name == input.value)) {
            validation = { isValid: false, errorMessage: "Name must be unique." };
        }
    }

    // handle styling
    console.log(validation);
    input.classList.toggle("validation-error", !validation.isValid);
    input.title = validation.errorMessage;

    return validation;
}

/**
 * TODO: learn this
 * @param {Function} func - Function to debounce
 * @param {number} wait - Delay in milliseconds
 * @returns {Function} Debounced function
 */
function debounce(func, wait) {
    let timeout;
    const debounced = function (...args) {
        clearTimeout(timeout);
        timeout = setTimeout(() => func.apply(this, args), wait);
    };
    debounced.cancel = () => clearTimeout(timeout);
    return debounced;
}