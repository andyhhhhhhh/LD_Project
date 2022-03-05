using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionController
{
    /// <summary>
    /// 虚拟卡
    /// </summary>
    class VirtualControl : Motor
    {
        public override int Motor_add_arc_pos(int crdID, double[] ppos1, double[] ppos2, double[] ppos3, TSpeed[] pspeed)
        {
            return (int)EM_ERRCODE.RETURN_OK;
        }

        public override int Motor_add_line_pos(int crdID, double[] ppos, TSpeed pspeed)
        {
            return (int)EM_ERRCODE.RETURN_OK;
        }

        public override int Motor_axis_disable(ushort cardIndex, ushort axisIndex)
        {
            return (int)EM_ERRCODE.RETURN_OK;
        }

        public override int Motor_axis_enable(ushort cardIndex, ushort axisIndex)
        {
            return (int)EM_ERRCODE.RETURN_OK;
        }

        public override int Motor_axis_home(ushort cardIndex, ushort axisIndex, uint homeIo, float speed, float secondSpeed, int homeType, int limitType)
        {
            return (int)EM_ERRCODE.RETURN_OK;
        }

        public override int Motor_axis_home_finished(ushort cardIndex, ushort axisIndex)
        {
            return (int)EM_ERRCODE.RETURN_OK;
        }

        public override int Motor_axis_is_moving(ushort cardIndex, ushort axisIndex)
        {
            return (int)EM_ERRCODE.RETURN_OK;
        }

        public override int Motor_axis_max_speed(ushort cardIndex, ushort axisIndex, out TSpeed speed)
        {
            speed = new TSpeed
            {
                acc = 100,
                dec = 100,
                vel = 100,
            };
            return (int)EM_ERRCODE.RETURN_OK;
        }

        public override int Motor_axis_move_offset(ushort cardIndex, ushort axisIndex, double offset, TSpeed pspeed, int basePos)
        {
            return (int)EM_ERRCODE.RETURN_OK;
        }

        public override int Motor_axis_move_pos(ushort cardIndex, ushort axisIndex, double pos, TSpeed pspeed)
        {
            return (int)EM_ERRCODE.RETURN_OK;
        }

        public override int Motor_axis_reset(ushort cardIndex, ushort axisIndex)
        {
            return (int)EM_ERRCODE.RETURN_OK;
        }

        public override int Motor_axis_stop(ushort cardIndex, ushort axisIndex, int type)
        {
            return (int)EM_ERRCODE.RETURN_OK;
        }

        public override int Motor_axis_vmove(ushort cardIndex, ushort axisIndex, int dir, TSpeed pspeed)
        {
            return (int)EM_ERRCODE.RETURN_OK;
        }

        public override int Motor_clear_crd_data(int crdID)
        {
            return (int)EM_ERRCODE.RETURN_OK;
        }

        public override int Motor_cmd(string pCmd)
        {
            return (int)EM_ERRCODE.RETURN_OK;
        }

        public override int Motor_crd_move(int crdID)
        {
            return (int)EM_ERRCODE.RETURN_OK;
        }

        public override int Motor_create_crd(out ushort pCardIndex, out ushort pAxisIndex, ushort axisCnt)
        {
            pAxisIndex = 1;
            pCardIndex = 1;
            return (int)EM_ERRCODE.RETURN_OK;
        }

        public override int Motor_deinit()
        {
            return (int)EM_ERRCODE.RETURN_OK;
        }

        public override int Motor_ext_read_in_bit(ushort cardIndex, ushort node, short index)
        {
            return (int)EM_ERRCODE.RETURN_OK;
        }

        public override int Motor_ext_write_out_bit(ushort cardIndex, short exindex, ushort ioindex, ushort val)
        {
            return (int)EM_ERRCODE.RETURN_OK;
        }

        public override int Motor_get_axis_status(ushort cardIndex, ushort axisIndex, int type)
        {
            return (int)EM_ERRCODE.RETURN_OK;
        }

        public override int Motor_get_axis_type(ushort cardIndex, ushort axisIndex)
        {
            return (int)EM_ERRCODE.RETURN_OK;
        }

        public override int Motor_get_crd_status(int crdID, out short pRun, out long pSegment)
        {
            pSegment = 1;
            pRun = 1;
            return (int)EM_ERRCODE.RETURN_OK;
        }

        public override int Motor_get_current_pos(ushort cardIndex, ushort axisIndex, out double pval, int type = 0)
        {
            pval = 99999;
            return (int)EM_ERRCODE.RETURN_OK; ;
        }

        public override int Motor_group_move(List<ushort> cardIndex, List<ushort> listAxisIndex, List<float> listpos, TSpeed speed, int stationIndex)
        {
            return (int)EM_ERRCODE.RETURN_OK;
        }

        public override int Motor_init_card(AxisParam paxis, ushort cardIndex, ushort axisCnt, int extIOCnt, int cntType)
        {
            return (int)EM_ERRCODE.RETURN_OK;
        }

        public override int Motor_read_adc(ushort cardIndex, ushort index, out double pvalue, short exindex)
        {
            pvalue = 0;
            return (int)EM_ERRCODE.RETURN_OK;
        }

        public override int Motor_read_in(ushort cardIndex, short di_type, out long pval, short exindex = 0)
        {
            pval = 0;
            return (int)EM_ERRCODE.RETURN_OK;
        }

        public override int Motor_read_in_bit(ushort cardIndex, ushort ioindex, short exindex)
        {
            return (int)EM_ERRCODE.RETURN_OK;
        }

        public override int Motor_read_out(ushort cardIndex, short do_type, out long pval, short exindex)
        {
            pval = 0;
            return (int)EM_ERRCODE.RETURN_OK;
        }

        public override int Motor_read_out_bit(ushort cardIndex, ushort ioindex, short exindex, short do_type)
        {
            return (int)EM_ERRCODE.RETURN_OK;
        }

        public override int Motor_reset()
        {
            return (int)EM_ERRCODE.RETURN_OK;
        }

        public override bool Motor_set_axis_zero_pos(ushort cardIndex, ushort axisIndex)
        {
            return true;
        }

        public override int Motor_write_out_bit(ushort cardIndex, ushort ioindex, ushort val, short exindex, short do_type)
        {
            return (int)EM_ERRCODE.RETURN_OK;
        }
    }
}
