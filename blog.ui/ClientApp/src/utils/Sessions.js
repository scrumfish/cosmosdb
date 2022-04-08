export const setSession = token => {
    sessionStorage.setItem('sessionToken', JSON.stringify(token))
    window.dispatchEvent(new Event('storage'))
}

export const getSession = () => {
    const token = sessionStorage.getItem('sessionToken')
    if (token) {
        return JSON.parse(token)
    }
    return null
}

export const deleteSession = () => {
    sessionStorage.removeItem('sessionToken')
    window.dispatchEvent(new Event('storage'))
}
