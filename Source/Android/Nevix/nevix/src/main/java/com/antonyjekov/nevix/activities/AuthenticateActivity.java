package com.antonyjekov.nevix.activities;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.widget.Toast;

import com.antonyjekov.nevix.R;
import com.antonyjekov.nevix.common.ContextManager;
import com.antonyjekov.nevix.common.HttpAsyncRequest;
import com.antonyjekov.nevix.common.PersistentManager;
import com.antonyjekov.nevix.fragments.AuthorizationFragment;

public class AuthenticateActivity extends BaseActivity {

    private PersistentManager persistant;
    public static final String SESSION_KEY_EXTRA = "com.antonyjekov.nevix.auth.sessionKey";
    private ContextManager data;
    private String sessionKey;

    private Context thisContext;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_authenticate);
        thisContext = this;
        data = new ContextManager(this);

        if (savedInstanceState == null) {
            this.persistant = new PersistentManager();

            getSupportFragmentManager().beginTransaction()
                    .add(R.id.container_auth, new AuthorizationFragment(new HttpAsyncRequest.OnResultCallBack() {
                        @Override
                        public void onResult(String result) {
                            if (!validateSessionKey(result)) {
                                warnUser("Wrong username or password!");
                                return;
                            }

                            handleLogin(result);
                            data.setSessionKey(result);
                        }
                    }))
                    .commit();
        }
    }

    private boolean validateSessionKey(String result) {
        return !result.startsWith("ttp:") && result.length() == 36;
    }

    private void warnUser(String message) {
        Toast.makeText(this, message, Toast.LENGTH_LONG).show();
    }

    @Override
    protected void onResume() {
        super.onResume();

        String sessionKey = data.getSessionKey();

        if (sessionKey != null && !sessionKey.equals(ContextManager.EMPTY_SESSION_KEY)) {
            handleLogin(sessionKey);
        }
    }

    private void handleLogin(String result) {
        this.sessionKey = result;
        data.setSessionKey(result);
        this.persistant.setSessionKey(result);
        finishLogin();
    }

    private void finishLogin() {
        Intent intent = new Intent(this, MainActivity.class);
        intent.putExtra(SESSION_KEY_EXTRA, this.sessionKey);

        startActivity(intent);
        finish();
    }
}