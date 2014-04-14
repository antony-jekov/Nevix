package com.antonyjekov.nevix.activities;

import android.content.Intent;
import android.content.pm.ActivityInfo;
import android.os.Bundle;
import android.view.ViewGroup;
import android.widget.Toast;

import com.antonyjekov.nevix.R;
import com.antonyjekov.nevix.common.PusherManager;
import com.antonyjekov.nevix.player.BSPlayer;
import com.antonyjekov.nevix.player.InteractionIndicator;
import com.antonyjekov.nevix.player.Player;

public class MainActivity extends BaseActivity {

    public static final int BROWSE_RESULT = 1001;

    private PusherManager pusher;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        getSupportActionBar().hide();

        this.setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE);

        String sessionKey = getIntent().getStringExtra(AuthenticateActivity.SESSION_KEY_EXTRA);

        if (sessionKey == null || sessionKey.equals("") || sessionKey.length() == 0) {
            throw new IllegalArgumentException("invalid session key.");
        }

        this.pusher = new PusherManager(sessionKey);

        ViewGroup root = (ViewGroup) findViewById(R.id.player_container);
        InteractionIndicator indicator = new InteractionIndicator(this);
        root.addView(indicator);
        Player player = new BSPlayer(this, pusher, this, indicator);
        root.addView(player);
    }

    public void browseMedia() {
        Intent browse = new Intent(this, BrowseActivity.class);
        startActivityForResult(browse, BROWSE_RESULT);
    }

    public void shutDownComputer(String cmd) {
        pusher.pushCommand(cmd);
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (resultCode == RESULT_OK) {
            if (requestCode == BROWSE_RESULT) {
                int requestedFile = data.getIntExtra(BrowseActivity.BROWSED_FILE, 0);
                if (requestedFile != 0) {
                    pusher.pushCommand("open" + requestedFile);
                }
            }
        }
    }
}