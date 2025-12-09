const statusList = document.getElementById('statusList');
const startButton = document.getElementById('startButton');
const logElement = document.getElementById('log');

const connection = new signalR.HubConnectionBuilder()
    .withUrl('/employeeStatusHub')
    .withAutomaticReconnect()
    .build();

connection.on('ReceiveStatusUpdate', (employeeId, status) => {
    updateStatusCard(employeeId, status);
    if (employeeId === 100 && status === 'done') {
        startButton.disabled = false;
        setLog('Processing complete for all employees.');
    }
});

connection.onreconnecting(() => setLog('Connection lost. Attempting to reconnect...'));
connection.onreconnected(() => setLog('Reconnected to employee status hub.'));
connection.onclose(() => setLog('Disconnected from employee status hub.'));

async function startConnection() {
    try {
        await connection.start();
        setLog('Connected. Ready to receive real-time updates.');
    } catch (error) {
        setLog('Failed to connect to SignalR hub. Retrying...');
        setTimeout(startConnection, 3000);
    }
}

function updateStatusCard(employeeId, status) {
    let item = document.getElementById(`employee-${employeeId}`);
    if (!item) {
        item = document.createElement('li');
        item.id = `employee-${employeeId}`;
        item.className = `status-card ${status}`;
        item.innerHTML = `<span class="status-label">Employee ${employeeId}</span><span class="status-text">${status}</span>`;
        statusList.appendChild(item);
    } else {
        item.className = `status-card ${status}`;
        item.querySelector('.status-text').textContent = status;
    }
}

async function startProcessing() {
    startButton.disabled = true;
    setLog('Starting processing for 100 employees...');
    try {
        const response = await fetch('/api/employee/process', { method: 'POST' });
        if (!response.ok) {
            throw new Error('Failed to start processing.');
        }
        setLog('Processing started. Watch the statuses update in real time.');
    } catch (error) {
        console.error(error);
        setLog('Unable to start processing. Please try again.');
        startButton.disabled = false;
    }
}

function setLog(message) {
    const timestamp = new Date().toLocaleTimeString();
    logElement.textContent = `[${timestamp}] ${message}`;
}

startConnection();
