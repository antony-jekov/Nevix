package com.antonyjekov.nevix.player.buttons;

import android.graphics.Path;
import android.graphics.Rect;

import com.antonyjekov.nevix.constants.PlayerCommand;

/**
 * Created by Antony Jekov on 4/1/2014.
 */
public class MuteButton extends SphericalButton {
    public MuteButton(Rect button, int strokeWidth, int padding) {
        super(button, strokeWidth, padding);
    }

    @Override
    protected void prepareShape(Path shape) {
        super.prepareShape(shape);

        int x = button.right - (padding << 1);
        int y = button.bottom - (padding);

        shape.moveTo(x, y);
        shape.lineTo(x, button.top + (padding));

        x -= strokeWidth << 1;
        y -= padding >> 1;

        shape.moveTo(x, y);
        shape.lineTo(x, (float) (button.top + (padding * 1.5)));

        x -= strokeWidth << 1;
        y -= padding >> 1;

        shape.moveTo(x, y);
        shape.lineTo(x, button.top + (padding << 1));

        shape.moveTo(button.left + (padding), button.bottom - (padding));
        shape.lineTo(button.right - (padding), button.top + (padding));
    }

    @Override
    public String command() {
        return PlayerCommand.MUTE_CMD;
    }
}
