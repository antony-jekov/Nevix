package com.antonyjekov.nevix.activities;

import android.content.Intent;
import android.content.pm.ActivityInfo;
import android.os.Bundle;
import android.support.v7.app.ActionBarActivity;
import android.view.Menu;
import android.view.MenuItem;
import android.view.ViewGroup;
import android.widget.Toast;

import com.antonyjekov.nevix.R;
import com.antonyjekov.nevix.common.ContextManager;
import com.antonyjekov.nevix.common.PersistentManager;
import com.antonyjekov.nevix.common.PusherManager;
import com.antonyjekov.nevix.player.BSPlayer;
import com.antonyjekov.nevix.player.Player;

public class MainActivity extends ActionBarActivity {

    public static final int BROWSE_RESULT = 1001;

    private PersistentManager persistent;
    private ContextManager data;
    private PusherManager pusher;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        this.setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE);

        String sessionKey = getIntent().getStringExtra(AuthenticateActivity.SESSION_KEY_EXTRA);

        if (sessionKey == null || sessionKey.equals("") || sessionKey.length() == 0) {
            throw new IllegalArgumentException("invalid session key.");
        }

        this.persistent = new PersistentManager(sessionKey);
        this.pusher = new PusherManager(sessionKey);

        data = new ContextManager(this);

        Player player = new BSPlayer(this, pusher);
        ((ViewGroup) findViewById(R.id.player_container)).addView(player);
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.main, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        Intent browse = new Intent(this, BrowseActivity.class);
        startActivityForResult(browse, BROWSE_RESULT);

        return true;
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (resultCode == RESULT_OK) {
            if (requestCode == BROWSE_RESULT) {
                int requestedFile = data.getIntExtra(BrowseActivity.BROWSED_FILE, 0);
                test(requestedFile + "");
                if (requestedFile != 0) {
                    pusher.pushCommand("open" + requestedFile);
                }
            }
        }
    }

    private void test(String text) {
        Toast.makeText(this, text, Toast.LENGTH_LONG).show();
    }
}