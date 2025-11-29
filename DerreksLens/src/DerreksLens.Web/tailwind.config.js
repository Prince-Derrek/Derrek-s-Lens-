// src/DerreksLens.Web/tailwind.config.js
/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        './Views/**/*.cshtml',
        './wwwroot/js/**/*.js'
    ],
    theme: {
        extend: {
            colors: {
                primary: '#0F172A',   // Slate-900 (Deep Blue)
                secondary: '#1E293B', // Slate-800
                accent: '#38BDF8',    // Sky-400
                danger: '#EF4444',    // Red-500
                success: '#22C55E',   // Green-500
                background: '#020617', // Slate-950 (Darkest)
                text: '#E5E7EB',      // Gray-200
            },
            fontFamily: {
                mono: ['ui-monospace', 'SFMono-Regular', 'Menlo', 'Monaco', 'Consolas', 'monospace'],
                sans: ['Inter', 'system-ui', 'sans-serif'],
            }
        },
    },
    plugins: [
        require('@tailwindcss/typography'),
    ], 
}