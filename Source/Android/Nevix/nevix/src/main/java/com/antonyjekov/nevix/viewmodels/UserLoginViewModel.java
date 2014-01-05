package com.antonyjekov.nevix.viewmodels;

public class UserLoginViewModel {
    private String email;
    private String password;

    public UserLoginViewModel(String email, String pass){
        this.email = email;
        this.password = pass;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }
}
