package com.antonyjekov.nevix.player.buttons;

import android.graphics.Path;
import android.graphics.Rect;

import com.antonyjekov.nevix.constants.PlayerCommand;

/**
 * Created by Antony Jekov on 4/1/2014.
 */
public class StopButton extends SphericalButton {
    public StopButton(Rect button) {
        super(button);
    }

    @Override
    protected void prepareShape(Path shape) {
        super.prepareShape(shape);
        int len = (button.width() - ((padding << 1) + (strokeWidth << 1)));
        int halfLen = len >> 1;
        int x = button.centerX() - halfLen;
        int y = button.centerY() - halfLen;

        shape.moveTo(x, y);
        shape.lineTo(x + len, y);
        shape.lineTo(x + len, y + len);
        shape.lineTo(x, y + len);
        shape.lineTo(x, y);
    }

    @Override
    public String command() {
        return PlayerCommand.STOP_CMD;
    }
}
