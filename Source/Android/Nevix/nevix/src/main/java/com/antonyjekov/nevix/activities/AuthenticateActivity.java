package com.antonyjekov.nevix.activities;

import android.content.Intent;
import android.content.SharedPreferences;
import android.support.v7.app.ActionBarActivity;
import android.support.v7.app.ActionBar;
import android.support.v4.app.Fragment;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.os.Build;
import android.webkit.CookieSyncManager;

import com.antonyjekov.nevix.R;
import com.antonyjekov.nevix.common.ContextManager;
import com.antonyjekov.nevix.common.PersistentManager;
import com.antonyjekov.nevix.common.contracts.IAsyncResponse;
import com.antonyjekov.nevix.fragments.AuthorizationFragment;

import org.apache.http.cookie.Cookie;

import java.net.CookieManager;
import java.net.CookieStore;
import java.util.List;

public class AuthenticateActivity extends ActionBarActivity implements IAsyncResponse {

    private PersistentManager persistant;
    public  static final String SESSION_KEY_EXTRA = "com.antonyjekov.nevix.auth.sessionKey";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_authenticate);

        if (savedInstanceState == null) {
            this.persistant = new PersistentManager(this);
            getSupportFragmentManager().beginTransaction()
                    .add(R.id.container_auth, new AuthorizationFragment(persistant))
                    .commit();
        }
    }

    @Override
    public void processFinish(String result) {
        Intent intent = new Intent();
        intent.putExtra(SESSION_KEY_EXTRA, result);

        setResult(RESULT_OK, intent);
        finish();
    }
}
