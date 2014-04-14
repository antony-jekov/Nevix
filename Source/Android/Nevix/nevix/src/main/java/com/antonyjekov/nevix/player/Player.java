package com.antonyjekov.nevix.player;

import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Picture;
import android.graphics.Rect;
import android.os.Vibrator;
import android.view.MotionEvent;
import android.view.View;

import com.antonyjekov.nevix.R;
import com.antonyjekov.nevix.activities.MainActivity;
import com.antonyjekov.nevix.common.PusherManager;
import com.antonyjekov.nevix.constants.PlayerCommand;
import com.antonyjekov.nevix.player.buttons.Button;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by Antony Jekov on 3/23/2014.
 */
public abstract class Player extends View  {

    protected Vibrator vibrator;

    protected int width;
    protected int height;

    protected int halfWidth;
    protected int halfHeight;

    private final PusherManager pusher;
    protected int buttonMargin;

    MainActivity owner;

    protected List<Button> buttons;

    @Override
    protected void onDraw(Canvas canvas) {
        super.onDraw(canvas);
        for (Button button : buttons)
            button.renderSelf(canvas);
    }

    @Override
    public boolean onTouchEvent(MotionEvent event) {
        float x = event.getX();
        float y = event.getY();
        int action = event.getAction();
        if (action == MotionEvent.ACTION_DOWN) {
            onClick(x, y);
        }

        return super.onTouchEvent(event);
    }

    @Override
    protected void onSizeChanged(int w, int h, int oldw, int oldh) {
        super.onSizeChanged(w, h, oldw, oldh);
        width = w;
        height = h;
        halfHeight = h >> 1;
        halfWidth = w >> 1;

        buttonMargin = (int) (width * .03);

        arrangeButtons();
    }

    protected abstract void arrangeButtons();

    private void onClick(float x, float y) {
        for (Button btn : buttons) {
            if (btn.isPointInButton(x, y)) {
                vibrator.vibrate(100);
                final String cmd = btn.command();
                if (cmd.equals(PlayerCommand.BROWSE_CMD))
                    owner.browseMedia();
                else if (cmd.equals(PlayerCommand.POWER_CMD)) {
                    new AlertDialog.Builder(owner)
                            .setTitle("Power off")
                            .setMessage("Do you really want to power off your computer?")
                            .setIcon(android.R.drawable.ic_dialog_alert)
                            .setPositiveButton(android.R.string.yes, new DialogInterface.OnClickListener() {

                                public void onClick(DialogInterface dialog, int whichButton) {
                                    owner.shutDownComputer(cmd);
                                }
                            })
                            .setNegativeButton(android.R.string.no, null).show();
                } else
                    pusher.pushCommand(btn.command());

                break;
            }
        }
    }

    public Player(Context context, PusherManager pusher, MainActivity owner) {
        super(context);
        this.pusher = pusher;
        buttons = new ArrayList<Button>();
        this.vibrator = (Vibrator) context.getSystemService(Context.VIBRATOR_SERVICE);
        this.owner = owner;
    }

    protected Rect rightTo(Rect relativeRect, int buttonSize) {
        int buttonHalfSize = buttonSize >> 1;
        int left = relativeRect.right + buttonMargin;
        return new Rect(left, relativeRect.centerY() - buttonHalfSize, left + (buttonHalfSize << 1), relativeRect.centerY() + buttonHalfSize);
    }

    protected Rect leftTo(Rect relativeRect, int buttonSize) {
        int halfSize = buttonSize >> 1;
        return new Rect(relativeRect.left - (buttonMargin + buttonSize), relativeRect.centerY() - halfSize, relativeRect.left - buttonMargin, relativeRect.centerY() + halfSize);
    }

    protected Rect onTopOf(Rect relativeRect, int buttonSize) {
        int halfSize = buttonSize >> 1;
        int centerX = relativeRect.centerX();
        int centerY = relativeRect.top - (buttonMargin + halfSize);

        return new Rect(centerX - halfSize, centerY - halfSize, centerX + halfSize, centerY + halfSize);
    }

    protected Rect bellowOf(Rect relativeRect, int buttonSize) {
        int halfSize = buttonSize >> 1;
        int centerX = relativeRect.centerX();
        int centerY = relativeRect.bottom + (buttonMargin + halfSize);

        return new Rect(centerX - halfSize, centerY - halfSize, centerX + halfSize, centerY + halfSize);
    }
}
