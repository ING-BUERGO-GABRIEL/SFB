
const platform = import.meta.env.VITE_PLATFORM

const apiUrl = (() => {
    switch (platform) {
        case "web":
            return window.location.origin
        case "maui":
            return import.meta.env.VITE_API_URL
        case "dev":
        default:
            return "http://localhost:5700/"
    }
})()


const config = {
    Sidebar_drawer: true,
    mini_sidebar: false,
    actTheme: 'dark',
    fontTheme: 'Public sans',
    apiUrl,
    platform,
}

export default config
