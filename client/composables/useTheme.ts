export default function useTheme() {
    const colorMode = useColorMode()

    const darkMode = computed(() => colorMode.value === 'dark')

    const theme = computed(() => {
        if (colorMode.preference === 'dark')
            return 'Dark'
        else if (colorMode.preference === 'light')
            return 'Light'
        return 'Auto'
    })

    async function setTheme(newTheme: 'Dark' | 'Light' | 'Auto') {
        if (newTheme === 'Dark')
            colorMode.preference = 'dark'
        else if (newTheme === 'Light')
            colorMode.preference = 'light'
        else if (newTheme === 'Auto')
            colorMode.preference = 'system'
    }

    return {
        theme,
        darkMode,
        setTheme,
    }
}
