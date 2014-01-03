package com.antonyjekov.nevix.fragments;

import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;

import com.antonyjekov.nevix.R;
import com.antonyjekov.nevix.common.PersisterManager;

public class LoginFragment extends Fragment {
    Button loginBtn;
    Button registerBtn;
    Button toggleBtn;

    EditText email;
    EditText pass;
    EditText confirm;

    String sessionKey;
    PersisterManager persister;
    Boolean isLogin;

    public static final String SESSION_KEY = "com.antonyjekov.nevix.login.sessionKey";

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        isLogin = true;
        persister = new PersisterManager();
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        final View loginView = inflater.inflate(R.layout.fragment_authenticate, container, false);

        loginBtn = (Button) loginView.findViewById(R.id.auth_login_btn);
        registerBtn = (Button) loginView.findViewById(R.id.auth_register_btn);

        email = (EditText)loginView.findViewById(R.id.auth_email);
        pass = (EditText)loginView.findViewById(R.id.auth_pass);
        confirm = (EditText)loginView.findViewById(R.id.auth_confirm);

        loginBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                String emailText = email.getText().toString();
                String passwordText = pass.getText().toString();

                sessionKey = persister.login(emailText, passwordText);
            }
        });

        registerBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                String emailText = email.getText().toString();
                String passwordText = pass.getText().toString();
                String confirmText = confirm.getText().toString();

                if (passwordText.equals(confirmText)){
                    sessionKey = persister.register(emailText, passwordText, confirmText);
                } else {
                    warnUser("Passwords do not match!");
                }
            }
        });

        toggleBtn = (Button) loginView.findViewById(R.id.auth_toggle);
        toggleBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (isLogin){
                    isLogin = false;
                    loginBtn.setVisibility(View.GONE);
                    confirm.setVisibility(View.VISIBLE);
                    registerBtn.setVisibility(View.VISIBLE);

                } else {
                    isLogin = true;
                    loginBtn.setVisibility(View.VISIBLE);
                    confirm.setVisibility(View.GONE);
                    registerBtn.setVisibility(View.GONE);
                }
            }
        });

        return loginView;
    }

    private void warnUser(String message) {

    }
}
