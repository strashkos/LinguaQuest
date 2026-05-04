// Theme Management
window.updateThemeToggleIcon = function updateThemeToggleIcon(theme) {
    const button = document.querySelector('.theme-toggle span');
    if (button) {
        button.textContent = theme === 'dark' ? '☀️' : '🌙';
    }
};

window.initTheme = function initTheme() {
    const savedTheme = localStorage.getItem('theme') || 'dark';
    document.documentElement.setAttribute('data-theme', savedTheme);
    updateThemeToggleIcon(savedTheme);
};

window.setTheme = function setTheme(theme) {
    document.documentElement.setAttribute('data-theme', theme);
    localStorage.setItem('theme', theme);
    updateThemeToggleIcon(theme);
};

window.toggleTheme = function toggleTheme() {
    const currentTheme = document.documentElement.getAttribute('data-theme') || localStorage.getItem('theme') || 'dark';
    const nextTheme = currentTheme === 'dark' ? 'light' : 'dark';
    setTheme(nextTheme);
};

// Auth modal helpers
window.showAuthModal = function showAuthModal() {
    const el = document.getElementById('auth-modal');
    if (el) el.classList.remove('hidden');
};

window.hideAuthModal = function hideAuthModal() {
    const el = document.getElementById('auth-modal');
    if (el) el.classList.add('hidden');
};

window.showAppAuthModal = function showAppAuthModal() {
    const el = document.getElementById('app-auth-modal');
    if (el) el.classList.add('open');
};

window.hideAppAuthModal = function hideAppAuthModal(event) {
    if (event) {
        event.stopPropagation();
    }

    const el = document.getElementById('app-auth-modal');
    if (el) el.classList.remove('open');
};

window.scrollToLevels = function scrollToLevels() {
    const el = document.querySelector('.level-path');
    if (el) {
        el.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
};

// Initialize theme on page load
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', initTheme);
} else {
    initTheme();
}
